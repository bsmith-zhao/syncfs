using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace util.rep
{
    public interface IDir
    {
        bool exist(string path);
        RepItem getItem(string path);

        Stream createFile(string path);
        Stream readFile(string path);
        Stream writeFile(string path);
        void deleteFile(string path);
        void moveFile(string src, string dst);

        void createDir(string path);
        void deleteDir(string path);
        void moveDir(string src, string dst);
    }
}
