using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using util.crypt;
using util.ext;
using util.rep;

namespace util.rep.aead
{
    public class AeadFsReposit : LocalDirReposit
    {
        public const string Type = "AeadFS";
        public const int Version = 2;
        public const string DirKeyDomain = "dir-key";

        public static string unifyId(string dir)
            => $"{Type}@{dir}";

        public override string ToString()
            => unifyId(repPath);

        public AeadFsReposit(string dir, AeadFsConf conf)
        {
            this.repPath = dir.pathUnify();
            this.conf = conf;

            rootDir.Create();
            pathSkip = rootDir.FullName.Length + 1;
            dirEnc = conf.newDirCrypt(DirKeyDomain.utf8());
            nameEnc = Encoding.GetEncoding(conf.Encode);
        }

        protected override string rootPath => $"{repPath}/0";
        public override string localPath => repPath;

        string repPath;
        AeadFsConf conf;
        int pathSkip;
        DirCrypt dirEnc;
        Encoding nameEnc;

        public override void createDir(string path)
        {
            var dir = locateToParent(path, out var name, true);
            if (null == name)
                throw new Error(this, "EmptyName", path);

            var sub = getSubItem(dir, name);
            if (sub == null)
                dir.CreateSubdirectory(settleName(dir, encryptName(name)));
            else if (!sub.isDir())
                throw new Error(this, "DstIsFile", path);
        }

        string settleName(DirectoryInfo dirInfo, string name)
        {
            var dir = dirInfo.FullName;
            if (!pathExist($"{dir}\\{name}"))
                return name;

            int idx = 0;
            while (idx++ < 1000)
            {
                var newName = $"{name}!{idx}";
                if (!pathExist($"{dir}\\{newName}"))
                    return newName;
            }
            throw new Error(this, "SettleOverflow", idx);
        }

        string settlePath(DirectoryInfo dirInfo, string name)
        {
            name = settleName(dirInfo, name);
            return $"{dirInfo.FullName}\\{name}";
        }

        bool pathExist(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        public override void moveDir(string src, string dst)
        {
            if (!locateToDir(src, out var srcItem))
                throw new Error(this, "DirNotExist", src);

            var dstDir = locateToParent(dst, out var dstName, true);
            if (null == dstName)
                throw new Error(this, "EmptyDstName", dst);
            var dstItem = getSubItem(dstDir, dstName);
            if (dstItem != null && src.ToLower() != dst.ToLower())
                throw new Error(this, "DstDirExist", dst);

            var encDstName = encryptName(dstName);

            if (srcItem.FullName == $"{dstDir.FullName}\\{encDstName}")
                return;

            var dstLoc = settlePath(dstDir, encDstName);
            srcItem.MoveTo(dstLoc);
        }

        public override void deleteDir(string path)
        {
            if (!locateToDir(path, out var dir))
                throw new Error(this, "DirNotExist", path);
            dir.Delete(true);
        }

        protected bool equalPath(string src, string dst)
        {
            return src.ToLower() == dst.ToLower()
                && src.pathName() == dst.pathName();
        }

        public override void moveFile(string src, string dst)
        {
            if (equalPath(src, dst))
                return;

            if (locateToFile(src, out var srcFile) == false)
                throw new Error(this, "FileNotExist", src);

            var dir = locateToParent(dst, out var dstName, true);
            if (null == dstName)
                throw new Error(this, "EmptyDstName", dst);

            // not rename, then check exist item
            if (src.ToLower() != dst.ToLower()
                && getSubItem(dir, dstName) != null)
                throw new Error(this, "DstFileExist", dst);

            var dstLoc = settlePath(dir, encryptName(dstName));

            srcFile.MoveTo(dstLoc);
        }

        public override void deleteFile(string path)
        {
            if (locateToFile(path, out var fi))
                fi.Delete();
            else
                throw new Error(this, "FileNotExist", path);
        }

        public override Stream createFile(string path)
        {
            var dir = locateToParent(path, out var name, true);
            if (null == name)
                throw new Error(this, "EmptyFileName", path);

            var node = getSubItem(dir, name);
            if (null != node)
                throw new Error(this, "FileExist", path);

            var encPath = settlePath(dir, encryptName(name));
            return new AeadFsStream(new FileStream(encPath,
                                    FileMode.CreateNew,
                                    FileAccess.ReadWrite,
                                    FileShare.Read | FileShare.Delete,
                                    conf.packSize()), conf, create: true);
        }

        protected override Stream openFile(string path, bool write)
        {
            if (!locateToFile(path, out var encFile))
                throw new Error(this, "FileNotExist", path);

            return new AeadFsStream(new FileStream(encFile.FullName,
                                    FileMode.Open,
                                    fileAccess(write),
                                    fileShare(write),
                                    conf.packSize()), conf, create: false);
        }

        string encryptName(string name)
        {
            byte[] data = nameEnc.GetBytes(name);
            bool utf8 = nameEnc.GetString(data) != name;
            if (utf8)
                data = name.utf8();

            var code = dirEnc.encrypt(data).b64()
                .TrimEnd('=').Replace('/', '%');

            if (data.Length == 16)
                code = $"{code}#";

            if (utf8)
                code = $"{code}$";

            return code;
        }

        public override string parseName(string name)
        {
            if (name.Length < 20// encrypt base64 min size
                || !decodeName(name, out var cipher, out var utf8, out var b16))
                return null;

            var data = dirEnc.decrypt(cipher, b16);

            return utf8 ? data.utf8() : nameEnc.GetString(data);
        }

        public bool decodeName(string name, out byte[] cipher, out bool utf8, out bool b16)
        {
            cipher = null;
            utf8 = false;
            b16 = false;
            int i = name.Length, end = name.Length;
            char[] cs = null;
            char c;
            while (i-- > 0)
            {
                switch (c = name[i])
                {
                    case '!':   // for conflict name postfix
                        end = i;
                        break;
                    case '$':   // for utf8 encoding
                        utf8 = true;
                        end = i;
                        break;
                    case '#':   // for utf8 encoding
                        b16 = true;
                        end = i;
                        break;
                    case '%':   // replace '/' in base64
                        cs = cs ?? name.ToCharArray();
                        cs[i] = '/';
                        break;
                    case '+': break;
                    default:    // invalid characters
                        if (!((c >= 'A' && c <= 'Z')
                            || (c >= 'a' && c <= 'z')
                            || (c >= '0' && c <= '9')))
                        {
                            return false;
                        }
                        break;
                }
            }

            if (null != cs)
                name = new string(cs, 0, end);
            else if (end != name.Length)
                name = name.Substring(0, end);

            cipher = name.b64();
            return true;
        }

        protected override DirectoryInfo addSubDir(DirectoryInfo dir,
                                                    string name)
            => getSubFile(dir, name) == null
            ? dir.CreateSubdirectory(settleName(dir, encryptName(name)))
            : throw new Error(this, "SubIsFile", name);

        protected override long getFileSize(FileInfo fi)
            => AeadFsStream.getDataSize(fi.Length, conf);

        public override string parsePath(FileSystemInfo fi)
            => fi?.FullName.TrimEnd('\\', '/').jump(pathSkip)
            ?.Split('\\').conv(n => parseName(n)).join("/");
    }
}
