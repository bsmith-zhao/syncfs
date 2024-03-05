using util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using util.ext;
using util.prop;

namespace util
{
    public static class Dialog
    {
        public static void dlgEvalTime(this string label, Action func)
        {
            try
            {
                var begin = DateTime.UtcNow;
                func();
                var span = $"{DateTime.UtcNow - begin}";
                if (!label.empty())
                    span = $"<{label}> {span}";
                span.dlgInfo();
            }
            catch (Exception err)
            {
                err.Message.dlgAlert();
            }
        }

        public static bool dlgEdit(this object args)
            => new PropDialog{ Args = args}.dialog();

        public static byte[] setPwd(this string msg)
        {
            var dlg = new PwdDialog{ title = msg }.setup();
            if (dlg.dialog())
                return dlg.newPwd;
            return null;
        }

        public static bool setPwd(this string msg, out byte[] pwd)
            => (pwd = setPwd(msg)) != null;

        public static byte[] setPwd<E>(this string msg)
            where E : Exception, new()
            => setPwd(msg, out var pwd) ? pwd : throw new E();

        public static bool modifyPwd(this string msg, 
            Func<byte[], bool> verify, out byte[] newPwd)
        {
            newPwd = null;
            var ui = new PwdDialog { title = msg }.modify(verify);
            if (ui.dialog())
                newPwd = ui.newPwd;
            else if (ui.error != null)
                throw ui.error;
            return newPwd != null;
        }

        public static bool queryPwd(this string msg, 
            Func<byte[], bool> verify)
        {
            var dlg = new PwdDialog { title = msg }.query(verify);
            if (dlg.dialog())
                return true;
            if (dlg.error != null)
                throw dlg.error;
            return false;
        }

        public static void queryPwd<E>(this string msg, 
            Func<byte[], bool> verify)
            where E : Exception, new()
        {
            if (!queryPwd(msg, verify))
                throw new E();
        }

        public static void queryPwd<E>(this Control ui, 
            string msg, Func<byte[], bool> verify)
            where E : Exception, new()
        {
            ui.safeRun(() => queryPwd<E>(msg, verify));
        }

        public static bool pickDir(this object src, out string path)
            => (path = src.pickDir()) != null;

        public static string pickDir(this object src)
        {
            var dlg = new PickDirDialog();
            if (dlg.ShowDialog() == true)
                return dlg.ResultPath.unify();
            return null;
        }

        public static bool pickFile(this object src, out string path, string flt = null)
        {
            path = null;
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = flt
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            path = dlg.FileName.unify();
            return true;
        }

        public static bool pickFiles(this Control ui, out string[] paths, string flt = null)
            => pickFiles(out paths, flt);

        public static bool pickFiles(out string[] paths, string flt = null)
        {
            paths = null;
            var dlg = new OpenFileDialog
            {
                Filter = flt,
                Multiselect = true,
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            paths = dlg.FileNames.unify();
            return true;
        }

        static string[] unify(this string[] paths)
            => paths.conv(p => unify(p));

        static string unify(this string path)
            => path.Replace('\\', '/');

        //public static bool saveFile(this object src, string name, out string path)
        //{
        //    path = null;
        //    SaveFileDialog dlg = new SaveFileDialog();
        //    dlg.FileName = name;
        //    if (dlg.ShowDialog() != DialogResult.OK)
        //    {
        //        return false;
        //    }
        //    path = dlg.FileName.unify();
        //    return true;
        //}

        public static bool saveFile(this object src, out string path, string filter = null, string name = null)
        {
            return (path = saveFile(filter, name)) != null;
        }

        public static string saveFile(string filter = null, string name = null)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = filter,
                FileName = name,
            };
            if (dlg.ShowDialog() != DialogResult.OK)
                return null;

            return dlg.FileName;
        }

        public static bool confirm(this string msg)
        {
            return MessageBox.Show(msg, "Dialog.Confirm".trans(), 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                == DialogResult.Yes;
        }

        public static void dlgInfo(this string msg)
        {
            MessageBox.Show(msg, "Dialog.Info".trans(), 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void dlgAlert(this string msg)
        {
            MessageBox.Show(msg, "Dialog.Alert".trans(), 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void dlgError(this string msg)
        {
            MessageBox.Show(msg, "Dialog.Error".trans(), 
                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }

    public class CancelPwd : Exception { }
}
