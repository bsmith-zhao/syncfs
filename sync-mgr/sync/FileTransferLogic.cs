using System;
using System.Collections.Generic;
using System.Linq;
using util;
using util.ext;
using sync.hash;
using sync.sync;
using sync.work;
using util.rep;

namespace sync.sync
{
    public class FileTransferLogic : Logic
    {
        public FileTransferLogic(Param args) => this.args = args;

        public Param args;
        public class Param
        {
            public IDir src;
            public IDir dst;
            
            public List<HashItem> adds;
            public List<HashItem> dels;
            public List<HashItem[]> moves;
        }

        public event Action<int, long> BeginTransfer;
        public event Action<HashItem> BeginDeleteFile;
        public event Action EndDeleteFile;
        public event Action<HashItem, string> BeginMoveFile;
        public event Action EndMoveFile;
        public event Action<HashItem> BeginCopyFile;
        public event Action<long> CopyFileUpdate;
        public event Action EndCopyFile;
        public event Action EndTransfer;

        public int totalCount 
            => args.adds.Count
            + args.dels.Count
            + args.moves.Count;

        public long copySize 
            => args.adds.Sum(u => u.size);

        public IDir srcRep => args.src;
        public IDir dstRep => args.dst;

        string relocFile(IDir rep, string path)
            => path.fsReloc(rep.exist, rep.moveFile);

        public override void start()
        {
            BeginTransfer?.Invoke(totalCount, copySize);

            // first move or delete the lost files
            foreach (var dst in args.dels)
            {
                BeginDeleteFile?.Invoke(dst);

                dstRep.deleteFile(dst.path);

                EndDeleteFile?.Invoke();
            }

            // then handle the moved or changed name files
            var moves = args.moves;
            var oldMap = moves.toMap(p => p[0].lowPath);
            foreach (var pair in moves)
            {
                var old = pair[0];
                var @new = pair[1];
                // move the file in dst folder
                BeginMoveFile?.Invoke(old, @new.path);

                var oldLow = old.lowPath;
                var newLow = @new.lowPath;
                // change name cases will not conflict
                if (oldLow != newLow)
                {
                    // conflict with other moving file
                    if (oldMap.pop(newLow, out var other))
                    {
                        var otherOld = other[0];
                        otherOld.path = relocFile(dstRep, otherOld.path);
                        oldMap.Add(otherOld.lowPath, other);
                    }
                }
                oldMap.Remove(oldLow);

                dstRep.moveFile(old.path, @new.path);

                EndMoveFile?.Invoke();
            }

            // copy files
            foreach (var src in args.adds)
            {
                BeginCopyFile?.Invoke(src);

                copyFile(srcRep, src, dstRep, delta => 
                {
                    checkCancel();

                    CopyFileUpdate?.Invoke(delta);
                });

                EndCopyFile?.Invoke();
            }

            EndTransfer?.Invoke();
        }

        byte[] copyBuff;
        void copyFile(IDir srcRep, FileItem src, IDir dstRep, Action<long> update)
        {
            using (var fin = srcRep.readFile(src.path))
            using (var fout = dstRep.createFile(src.path))
            {
                copyBuff = copyBuff ?? new byte[4 * 1024 * 1024];
                int size = 0;
                while ((size = fin.Read(copyBuff, 0, copyBuff.Length)) > 0)
                {
                    fout.Write(copyBuff, 0, size);
                    update?.Invoke(size);
                }
            }
        }
    }
}
