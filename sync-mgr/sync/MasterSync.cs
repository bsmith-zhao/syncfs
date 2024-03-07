using sync.hash;
using sync.work;
using System;
using System.Windows.Forms;
using util;
using util.ext;
using util.rep;

namespace sync.sync
{
    public class MasterSync : Work
    {
        public bool parse;
        public Param args;
        public class Param
        {
            public IDir src;
            public IDir dst;

            public string srcOut;
            public string dstOut;

            public Hash hash;
            public Compare comp;
        }

        public override string ToString()
            => args.desc();

        public IDir srcRep => args.src;
        public IDir dstRep => args.dst;
        public Hash hash => args.hash;

        public string srcFilesPath => $"{args.srcOut}/files";
        public string dstFilesPath => $"{args.dstOut}/files";
        public string compCachePath => $"{dir}/equals";

        public FileDiffLogic fileDiff;
        public DirDiffLogic dirDiff;

        public void diffParse()
        {
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

            args.comp.cache = compCachePath;
            fileDiff = run(new FileDiff(new FileDiffLogic.Param
            {
                src = srcRep,
                dst = dstRep,
                comp = args.comp,
                srcFiles = srcFiles,
                dstFiles = dstFiles,
            })).lgc;

            // dir diffs
            dirDiff = run(new DirDiff(new DirDiffLogic.Param
            {
                src = srcRep,
                dst = dstRep,
            })).lgc;
        }

        public override void begin()
        {
            diffParse();
            showTrans();
            if (!parse)
                work();
        }

        public override void end()
            => args.comp.saveCache();

        void showTrans()
        {
            if (PanelOutput == null)
                return;
            var dstTree = TransSummary.parse(fileDiff);
            dstTree.name = $"{dstRep}";
            PanelOutput.asyncCall(() => 
            {
                var panel = new TransSumPanel(dstTree);
                panel.Dock = DockStyle.Fill;
                PanelOutput.Controls.Add(panel);
            });
        }

        void work()
        {
            run(new FileTransfer(new FileTransferLogic.Param
            {
                src = srcRep,
                dst = dstRep,
                adds = fileDiff.incrs,
                dels = fileDiff.lacks,
                moves = fileDiff.moves.reverse(),
            }));

            // adjust folder
            run(new DirAdjust(new DirAdjustLogic.Param
            {
                rep = dstRep,
                adds = dirDiff.incrs,
                dels = dirDiff.lacks,
                moves = dirDiff.moves.reverse(),
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

            run(new ConsistCheck(new ConsistCheckLogic.Param
            {
                src = srcRep,
                dst = dstRep,
                srcFiles = srcFiles,
                dstFiles = dstFiles,
                comp = args.comp,
            }));
        }
    }
}
