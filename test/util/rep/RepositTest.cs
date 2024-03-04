using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using util;
using util.ext;
using util.rep;

namespace test
{
    public class RepositTest : Test
    {
        public Reposit rep;

        public override void test()
        {
            using (rep)
            {
                //rep.open();

                fileTest("f");
                fileTest("x/y/z");

                dirTest("a");
                dirTest("b/c/d");

                $"enumDirs/enumFiles: by null".msg();
                var nt = getDirTree(null);
                nt.msg();

                $"enumDirs/enumFiles: by /".msg();
                var st = getDirTree("/");
                st.msg();

                $"enumDirs/enumFiles: by \"\"".msg();
                var et = getDirTree("");
                et.msg();
                assert(null != nt 
                    && nt.Length > 0 
                    && nt == st 
                    && st == et);
            }
        }

        public void fileTest(string path)
        {
            //Test1.begin();

            path = mixPath(path);

            $"addFile: add".@case();
            call(addFile, path);
            var filePath = path + "-file";
            call(addFile, filePath);

            $"addFile: by self/up/low".@case();
            assertError(addFile, path);
            assertError(addFile, path.ToLower());
            assertError(addFile, path.ToUpper());

            $"addFile: file as dir".@case();
            assertError(addFile, path+"/na");

            $"addFile: by dir path".@case();
            var dirPath = path + "-dir";
            rep.createDir(dirPath);
            assertError(addFile, dirPath);

            $"exist: not exist".@case();
            //assert(rep.exist(path + "-na") == false);

            $"exist: by self/up/low".@case();
            fileExistAssert(path);
            fileExistAssert(path.ToUpper());
            fileExistAssert(path.ToLower());

            $"getItem: not exist".@case();
            //assert(rep.getItem(path + "-na") == null);

            $"getItem: by self/up/low".@case();
            getFileAssert(path, path);
            getFileAssert(path.ToLower(), path);
            getFileAssert(path.ToUpper(), path);

            $"readFile: by self/up/low".@case();
            call(readFile, path);
            call(readFile, path.ToLower());
            call(readFile, path.ToUpper());

            $"writeFile: by self/up/low".@case();
            call(writeFile, path);
            call(writeFile, path.ToLower());
            call(writeFile, path.ToUpper());

            $"moveFile: from self/up/low".@case();
            call(rep.moveFile, path, path);
            call(rep.moveFile, path.ToUpper(), path);
            getFileAssert(path.ToUpper(), path);
            call(rep.moveFile, path.ToLower(), path);
            getFileAssert(path.ToLower(), path);

            $"moveFile: to dir.upper/name".@case();
            noneChangeAssert(null, () =>
            {
                var name = path.pathName();
                var newPath = path.cut(name.Length).ToUpper() + name;
                call(rep.moveFile, path, newPath);
                return null;
            });

            $"moveFile: to dir.lower/name".@case();
            noneChangeAssert(null, () =>
            {
                var name = path.pathName();
                var newPath = path.cut(name.Length).ToLower() + name;
                call(rep.moveFile, path, newPath);
                return null;
            });

            $"moveFile: change name".@case();
            {
                var newPath = path + path.pathName();
                call(rep.moveFile, path, newPath);
                notExistAssert(path);
                fileExistAssert(newPath);
                path = newPath;
            }

            $"moveFile: to name.upper".@case();
            {
                var name = path.pathName();
                var newPath = path.cut(name.Length) + name.ToUpper();
                call(rep.moveFile, path, newPath);
                getFileAssert(path, newPath);
                path = newPath;
            }

            $"moveFile: to name.lower".@case();
            {
                var name = path.pathName();
                var newPath = path.cut(name.Length) + name.ToLower();
                call(rep.moveFile, path, newPath);
                getFileAssert(path, newPath);
                path = newPath;
            }

            $"moveFile: to not exist".@case();
            {
                var newPath = path.Split('/').join("1/") + "1";
                call(rep.moveFile, path, newPath);
                notExistAssert(path);
                fileExistAssert(newPath);
                path = newPath;
            };

            $"moveFile: from not exist".@case();
            assertError(rep.moveFile, path + "-na1", path + "-na2");

            $"moveFile: to exist file/dir".@case();
            {
                var newPath = path.Split('/').join("2/") + "2";
                call(addFile, newPath);
                assertError(rep.moveFile, path, newPath);
                assertError(rep.moveFile, path, dirPath);
            }

            $"moveFile: to sub".@case();
            {
                var newPath = path + "/" + path.pathName() + "-na";
                assertError(rep.moveFile, path, newPath);
            }

            $"moveFile: to file as dir".@case();
            assertError(rep.moveFile, path, filePath+"/na");

            $"deleteFile: by not exist".@case();
            assertError(rep.deleteFile, path + "-na");

            $"deleteFile: delete".@case();
            call(rep.deleteFile, path);
            notExistAssert(path);
        }

        void addFile(string path)
        {
            using (var fout = rep.createFile(path)) { }
        }

        void readFile(string path)
        {
            using (var fin = rep.readFile(path)) { }
        }

        void writeFile(string path)
        {
            using (var fout = rep.writeFile(path)) { }
        }

        string mixPath(string path)
        {
            var ns = new List<string>();
            foreach (var nd in path.Split('/'))
                ns.Add(nd.ToLower() + nd.ToUpper());
            return string.Join("/", ns);
        }

        public void dirTest(string path)
        {
            //Test1.begin();

            //path = mixPath(path);

            //$"addDir: add".@case();
            //call(rep.addDir, path);

            //var filePath = path + "-f";
            //call(addFile, filePath);
            //$"addDir: file as dir".@case();
            //assertError(rep.addDir, filePath + "/na");

            //$"addDir: by self/up/low".@case();
            //noneChangeAssert(null, () =>
            //{
            //    call(rep.addDir, path);
            //    call(rep.addDir, path.ToLower());
            //    call(rep.addDir, path.ToUpper());
            //    return null;
            //});

            //$"exist: not exist".@case();
            //assert(rep.exist(path+ "-na") == false);

            //$"exist: by self/up/low".@case();
            //dirExistAssert(path);
            //dirExistAssert(path.ToUpper());
            //dirExistAssert(path.ToLower());

            //$"getItem: not exist".@case();
            //assert(rep.getItem(path + "-na") == null);

            //$"getItem: by self/up/low".@case();
            //getDirAssert(path, path);
            //getDirAssert(path.ToLower(), path);
            //getDirAssert(path.ToUpper(), path);

            //var rs = new HashSet<string>();
            //{
            //    var tk = path.cut(path.locName().Length);
            //    int cnt = 3;
            //    while (cnt-- > 0)
            //    {
            //        {
            //            var sub = tk + cnt;
            //            rep.addDir(sub);
            //            rs.Add(sub);
            //        }
            //        {
            //            var sub = tk + cnt + "/" + cnt;
            //            rep.addDir(sub);
            //            rs.Add(sub);
            //        }
            //    }
            //    rs.Add(path);
            //}
            
            //$"enumDirs: on root".@case();
            //{
            //    var cs = new HashSet<string>(rs);
            //    rep.enumDirs(null, true, d => cs.Remove(d));
            //    assert(cs.Count == 0);
            //}
            
            //$"enumDirs: on path.dir".@case();
            //{
            //    var cs = new HashSet<string>(rs);
            //    rep.enumDirs(path.locDir(), true, d => cs.Remove(d));
            //    assert(cs.Count == 0);
            //}

            //$"enumDirs: on path.dir.lower".@case();
            //{
            //    var cs = new HashSet<string>(rs);
            //    rep.enumDirs(path.locDir()?.ToLower(), true, d => cs.Remove(d));
            //    assert(cs.Count == 0);
            //}

            //$"enumDirs: on path.dir.upper".@case();
            //{
            //    var cs = new HashSet<string>(rs);
            //    rep.enumDirs(path.locDir()?.ToUpper(), true, d => cs.Remove(d));
            //    assert(cs.Count == 0);
            //}

            //$"moveDir: from self/up/low".@case();
            //call(rep.moveDir, path, path);
            //call(rep.moveDir, path.ToUpper(), path);
            //getDirAssert(path.ToUpper(), path);
            //call(rep.moveDir, path.ToLower(), path);
            //getDirAssert(path.ToLower(), path);

            //$"moveDir: to dir.upper/name".@case();
            //noneChangeAssert(null, () =>
            //{
            //    var name = path.locName();
            //    var newPath = path.cut(name.Length).ToUpper() + name;
            //    call(rep.moveDir, path, newPath);
            //    return null;
            //});

            //$"moveDir: to dir.lower/name".@case();
            //noneChangeAssert(null, () =>
            //{
            //    var name = path.locName();
            //    var newPath = path.cut(name.Length).ToLower() + name;
            //    call(rep.moveDir, path, newPath);
            //    return null;
            //});

            //$"moveDir: change name".@case();
            //{
            //    var newPath = path + path.locName();
            //    call(rep.moveDir, path, newPath);
            //    notExistAssert(path);
            //    dirExistAssert(newPath);
            //    path = newPath;
            //}

            //$"moveDir: to name.upper".@case();
            //{
            //    var name = path.locName();
            //    var newPath = path.cut(name.Length) + name.ToUpper();
            //    call(rep.moveDir, path, newPath);
            //    getDirAssert(path, newPath);
            //    path = newPath;
            //}

            //$"moveDir: to name.lower".@case();
            //{
            //    var name = path.locName();
            //    var newPath = path.cut(name.Length) + name.ToLower();
            //    call(rep.moveDir, path, newPath);
            //    getDirAssert(path, newPath);
            //    path = newPath;
            //}

            //$"moveDir: to not exist".@case();
            //call(rep.addDir, path+"/"+path.locName()+"-sub");
            //noneChangeAssert(path, () =>
            //{
            //    var newPath = path.Split('/').join("1/") + "1";
            //    call(rep.moveDir, path, newPath);
            //    notExistAssert(path);
            //    dirExistAssert(newPath);
            //    path = newPath;
            //    return newPath;
            //});

            //$"moveDir: from not exist".@case();
            //assertError(rep.moveDir, path + "-na1", path + "-na2");

            //$"moveDir: to exist".@case();
            //{
            //    var newPath = path.Split('/').join("2/") + "2";
            //    call(rep.addDir, newPath);
            //    assertError(rep.moveDir, path, newPath);
            //    assertError(rep.moveDir, path, filePath);
            //}

            //$"moveDir: to file as dir".@case();
            //assertError(rep.moveDir, path, filePath+"/na");

            //$"moveDir: to sub".@case();
            //{
            //    var newPath = path + "/" + path.locName()+"-na";
            //    assertError(rep.moveDir, path, newPath);
            //}

            //$"deleteDir: by not exist".@case();
            //assertError(rep.deleteDir, path + "-na", true);
            //assertError(rep.deleteDir, path + "-na", false);

            //$"deleteDir: on not empty by recurse = false".@case();
            //rep.addDir(path.locMerge(path.locName()));
            //assertError(rep.deleteDir, path, false);

            //$"deleteDir: on not empty by recurse = true".@case();
            //call(rep.deleteDir, path, true);
            //notExistAssert(path);
        }

        public void dirExistAssert(string path) { }
        //=> rep.existDir(path).assert();

        public void fileExistAssert(string path)
        { }
        //=> rep.existFile(path).assert();

        public void notExistAssert(string path) { }
            //=> (rep.exist(path) == false).assert();

        public void getItemAssert(string path, string realPath, bool type)
        {
            var item = rep.getItem(path);
            $"n{item.path} vs r{realPath}".msg();
            (null != item 
            && item.path == realPath 
            && !item.isDir == type).assert();
        }

        public void getDirAssert(string path, string realPath)
            => getItemAssert(path, realPath, false);

        public void getFileAssert(string path, string realPath)
            => getItemAssert(path, realPath, true);

        public void noneChangeAssert(string path, Func<string> func)
        {
            var ot = getDirTree(path);
            var np = func() ?? path;
            var nt = getDirTree(np);
            $"old[{ot}] vs new[{nt}]".msg();
            assert(nt.Length > 0 && nt == ot);
        }

        public string getDirTree(string dir)
        {
            //var ps = new List<string>();
            //rep.enumDirs(dir, true, d => ps.Add(d));
            //rep.enumFiles<FileItem>(dir, true, f => ps.Add(f.path));

            //var ns = new List<string>();
            //var pre = dir?.TrimStart('/')?.Length > 0 ? dir.Length + 1: 0;
            //ps.ForEach(p => ns.Add(p.jump(pre)));
            //return string.Join(",", ns);
            return null;
        }
    }
}
