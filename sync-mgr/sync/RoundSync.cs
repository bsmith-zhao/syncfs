using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using sync.hash;
using sync.work;
using util.ext;
using System.IO;
using System.Windows.Forms;

namespace sync.sync
{
    public class RoundSync : MasterSync
    {
        public string lastFilesPath => dir.pathMerge("last-files");
        public string lastDirsPath => dir.pathMerge("last-dirs");
        public string unsolvesPath => dir.pathMerge("unsolves.txt");

        public RoundDiffLogic interDiff;

        public void interParse()
        {
            diffParse();

            interDiff = run(new RoundDiff(new RoundDiffLogic.Param
            {
                src = srcRep,
                dst = dstRep,

                fileIncrs = fileDiff.incrs,
                fileLacks = fileDiff.lacks,
                fileMoves = fileDiff.moves,

                dirIncrs = dirDiff.incrs,
                dirLacks = dirDiff.lacks,
                dirMoves = dirDiff.moves,

                lastFiles = hash.loadUnits(lastFilesPath),
                lastDirs = lastDirsPath.readList(),
            })).lgc;
        }

        bool unsolved => interDiff.unsolvedCount > 0;

        public override void begin()
        {
            interParse();
            showTrans();
            if (unsolved)
            {
                showUnsolves();
                throw new Error(this, "Unsolved",
                    interDiff.unsolvedCount);
            }
            if (!parse)
            {
                work();
            }
        }

        void work()
        {
            run(new FileTransfer(new FileTransferLogic.Param
            {
                src = srcRep,
                dst = dstRep,
                adds = interDiff.fileTrans.dst.adds,
                dels = interDiff.fileTrans.dst.dels,
                moves = interDiff.fileTrans.dst.moves,
            }));

            run(new FileTransfer(new FileTransferLogic.Param
            {
                src = dstRep,
                dst = srcRep,
                adds = interDiff.fileTrans.src.adds,
                dels = interDiff.fileTrans.src.dels,
                moves = interDiff.fileTrans.src.moves,
            }));

            // adjust src dirs
            run(new DirAdjust(new DirAdjustLogic.Param
            {
                rep = srcRep,
                adds = interDiff.dirTrans.src.adds,
                dels = interDiff.dirTrans.src.dels,
                moves = interDiff.dirTrans.src.moves,
            }));

            // adjust dst dirs
            run(new DirAdjust(new DirAdjustLogic.Param
            {
                rep = dstRep,
                adds = interDiff.dirTrans.dst.adds,
                dels = interDiff.dirTrans.dst.dels,
                moves = interDiff.dirTrans.dst.moves,
            }));

            // consist check
            var srcFiles = run(new ComputeHash(new ComputeHashLogic.Param
            {
                src = srcRep,
                hash = hash,
                dst = srcFilesPath,
            })).lgc.files;

            var dstFiles = run(new ComputeHash(new ComputeHashLogic.Param
            {
                src = dstRep,
                hash = hash,
                dst = dstFilesPath,
            })).lgc.files;

            var check = run(new ConsistCheck(new ConsistCheckLogic.Param
            {
                src = srcRep,
                dst = dstRep,
                srcFiles = srcFiles,
                dstFiles = dstFiles,
                comp = args.comp,
            })).lgc;

            hash.saveUnits(srcFiles, lastFilesPath);
            check.dstDirs.saveTextTo(lastDirsPath);
        }

        void showTrans()
        {
            if (PanelOutput == null)
                return;
            var srcTree = TransSummary.parse(interDiff.fileTrans.src);
            srcTree.name = $"{srcRep}";
            var dstTree = TransSummary.parse(interDiff.fileTrans.dst);
            dstTree.name = $"{dstRep}";
            PanelOutput.asyncCall(() => 
            {
                var ui = new TransSumPanel(srcTree, dstTree);
                ui.Dock = DockStyle.Fill;
                PanelOutput.Controls.Add(ui);
            });
        }

        void showUnsolves()
        {
            var buff = new List<string>();
            outputUnsolves(buff.Add, 20);
            msg(buff.join("\r\n"));

            saveUnsolves();
        }

        void saveUnsolves()
        {
            using (var fout = File.CreateText(unsolvesPath))
            {
                outputUnsolves(fout.WriteLine);
            }
        }

        void outputUnsolves(Action<string> output, int? limit = null)
        {
            //output($"[{this.trans("Source")}] {srcRep.uri}");
            //output($"[{this.trans("Target")}] {dstRep.uri}");
            output("");

            outputPairs(toPaths(interDiff.fileMoveConfuses),
                this.trans("FileMoveConfuses"), output, limit);
            output("");

            outputPairs(toPaths(interDiff.fileNameConflicts),
                this.trans("FileNameConflicts"), output, limit);
            output("");

            outputPairs(interDiff.dirMoveConfuses,
                this.trans("DirMoveConfuses"), output, limit);
            output("");
        }

        List<string[]> toPaths(List<HashItem[]> pairs)
            => pairs.conv(p => new string[] { p[0].path, p[1].path });

        void outputPairs(List<string[]> pairs, string name,
            Action<string> output, int? limit = null)
        {
            output($"[{name}]\t{pairs.Count}");
            limit = limit ?? pairs.Count;
            int n = 0;
            foreach (var p in pairs)
            {
                if (n++ >= limit)
                    break;
                output($"{p[0]} - {p[1]}");
            }
            if (n < pairs.Count)
                output($"({pairs.Count - n} ...)");
        }
    }
}
