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
        public const string DirKeyContext = "dir-key";

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
            dirEnc = conf.newDirCrypt(DirKeyContext.utf8());
            textEnc = Encoding.GetEncoding(conf.Encode);
        }

        protected override string rootPath => $"{repPath}/0";
        public override string localPath => repPath;

        string repPath;
        AeadFsConf conf;
        int pathSkip;
        DirCrypt dirEnc;
        Encoding textEnc;

        public override void createDir(string path)
        {
            var dir = locateToParent(path, out var name, 
                out var realDir, create: true);
            if (null == name)
                throw new Error(this, "EmptyName", path);

            var sub = getSubItem(dir, name, out var realName);
            if (sub == null)
                dir.CreateSubdirectory(settleLongName(dir, encodeName(name)));
            else if (!sub.isDir())
                throw new Error(this, "PathExist", path);
        }

        public override void moveDir(string src, string dst)
        {
            if (!locateToDir(src, out var srcItem))
                throw new Error(this, "DirNotExist", src);

            var dstDir = locateToParent(dst, out var dstName, 
                out var realDir, create: true);
            if (null == dstName)
                throw new Error(this, "EmptyDstName", dst);
            var dstItem = getSubItem(dstDir, dstName, out var realName);
            if (dstItem != null && src.ToLower() != dst.ToLower())
                throw new Error(this, "DstPathExist", dst);

            var encDstName = encodeName(dstName);

            if (srcItem.FullName == $"{dstDir.FullName}\\{encDstName}")
                return;

            var dstPath = settlePath(dstDir, encDstName);
            deleteLongName(srcItem, () => srcItem.MoveTo(dstPath));
        }

        public override void deleteDir(string path)
        {
            if (!locateToDir(path, out var dir))
                throw new Error(this, "DirNotExist", path);

            deleteLongName(dir, () => dir.Delete(true));
        }

        public override void moveFile(string src, string dst)
        {
            if (equalPath(src, dst))
                return;

            if (locateToFile(src, out var srcFile) == false)
                throw new Error(this, "FileNotExist", src);

            var dir = locateToParent(dst, out var dstName, 
                out var realDir, create: true);
            if (null == dstName)
                throw new Error(this, "EmptyDstName", dst);

            // not rename, then check exist item
            if (src.ToLower() != dst.ToLower()
                && getSubItem(dir, dstName, out var realName) != null)
                throw new Error(this, "DstPathExist", dst);

            var dstPath = settlePath(dir, encodeName(dstName));

            deleteLongName(srcFile, () => srcFile.MoveTo(dstPath));
        }

        public override void deleteFile(string path)
        {
            if (locateToFile(path, out var file))
                deleteLongName(file, () => file.Delete());
            else
                throw new Error(this, "FileNotExist", path);
        }

        public override Stream createFile(string path)
        {
            var dir = locateToParent(path, out var name, 
                out var realDir, create: true);
            if (null == name)
                throw new Error(this, "EmptyName", path);

            var node = getSubItem(dir, name, out var realName);
            if (null != node)
                throw new Error(this, "PathExist", path);

            var encPath = settlePath(dir, encodeName(name));
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

        protected override DirectoryInfo addSubDir(DirectoryInfo dir,
                                                    string name)
            => getSubFile(dir, name) == null
            ? dir.CreateSubdirectory(settleLongName(dir, encodeName(name)))
            : throw new Error(this, "SubIsFile", name);

        protected override long getFileSize(FileInfo fi)
            => AeadFsStream.getDataSize(fi.Length, conf);

        bool equalPath(string src, string dst)
        {
            return src.ToLower() == dst.ToLower()
                && src.pathName() == dst.pathName();
        }

        void deleteLongName(FileSystemInfo item,
            Action func)
        {
            var path = item.FullName;
            func();

            if (path.lastIdx('&', 10) > 0)
                File.Delete($"{path}~");
        }

        const int MaxName = 250;

        string settleLongName(DirectoryInfo dir, string name)
        {
            string longName = null;
            if (name.Length > MaxName)
            {
                longName = name.Replace("%", "/");
                if (longName.last() == '$')
                    longName = longName.cut(1);

                name = $"{name.tail(MaxName)}&";
            }

            name = settleName(dir, name);

            if (longName != null)
            {
                File.WriteAllText($"{dir.FullName}\\{name}~", longName);
            }

            return name;
        }

        string settleName(DirectoryInfo dir, string name)
        {
            var dirPath = dir.FullName;
            if (!pathExist($"{dirPath}\\{name}"))
                return name;

            int idx = 0;
            while (idx++ < 1000)
            {
                var newName = $"{name}!{idx}";
                if (!pathExist($"{dirPath}\\{newName}"))
                    return newName;
            }
            throw new Error(this, "SettleOverflow", idx);
        }

        string settlePath(DirectoryInfo dir, string name)
        {
            name = settleLongName(dir, name);
            return $"{dir.FullName}\\{name}";
        }

        bool pathExist(string path)
        {
            return File.Exists(path) || Directory.Exists(path);
        }

        string encodeName(string name)
        {
            byte[] data = textEnc.GetBytes(name);
            bool utf8 = textEnc.GetString(data) != name;
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

        public override string parseName(FileSystemInfo item)
        {
            var name = item.Name;
            // long name store file
            if (name.EndsWith("~"))
                return null;
            // 16-bytes base64 min size
            if (name.Length < 20)
                return null;
            // filter mark char and check invalid char
            if (!decodeName(name, out var cipher,
                                out var utf8, out var b16,
                                out var isLong))
                return null;

            if (isLong)
            {
                // read and decode long name
                var dir = item.FullName.cut(name.Length + 1);
                var longName = File.ReadAllText($"{dir}\\{name}~");
                cipher = longName.b64();
            }

            var data = dirEnc.decrypt(cipher, b16);

            return utf8 ? data.utf8() : textEnc.GetString(data);
        }

        bool decodeName(string name, 
            out byte[] cipher, 
            out bool utf8, out bool b16,
            out bool isLong)
        {
            cipher = null;
            utf8 = false;
            b16 = false;
            isLong = false;

            int i = name.Length, end = name.Length;
            char[] cs = null;
            char c;
            while (i-- > 0)
            {
                switch (c = name[i])
                {
                    case '&':
                        end = i;
                        isLong = true;
                        break;
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
                    default:    // check invalid characters
                        if (!((c >= 'A' && c <= 'Z')
                            || (c >= 'a' && c <= 'z')
                            || (c >= '0' && c <= '9')))
                        {
                            return false;
                        }
                        break;
                }
            }

            if (isLong)
                return true;

            if (null != cs)
                name = new string(cs, 0, end);
            else if (end != name.Length)
                name = name.Substring(0, end);

            cipher = name.b64();
            return true;
        }
    }
}
