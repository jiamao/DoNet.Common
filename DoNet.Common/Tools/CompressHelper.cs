using System;
using System.Collections.Generic;
//////////////////////////////////////////////////
// Author   : 丁峰峰
// Date     : 2010/09/15
// Usage    : 压缩
//////////////////////////////////////////////////

using System.Text;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace DoNet.Common.Tool
{
    public class Gzip
    {
        /// <summary>
        /// 打包单个文件
        /// </summary>
        /// <param name="TargetFile"></param>
        /// <param name="sourceFile"></param>
        public void SerFileZip(string TargetFile, string sourceFile)
        {
            FileStream SourceFileStream = null;
            GZipStream CompressedStream = null;//压缩流  
            FileStream SerFile = new FileStream(TargetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                Console.WriteLine("目标文件：" + TargetFile);

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);

                Console.Write("打包:" + System.IO.Path.GetFileName(sourceFile) + "           ");
                SourceFileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                long packedsize = 0;
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int re = SourceFileStream.Read(buffer, 0, buffer.Length);//读取源文件 
                    if (re > 0)
                    {
                        CompressedStream.Write(buffer, 0, re);//压缩 
                        packedsize += re;
                        float per = (float)packedsize / SourceFileStream.Length * 100;
                        Console.Write("\b\b\b\b{0,4:G}", per.ToString("###") + "%");
                    }
                    else
                    {
                        Console.WriteLine("\b\b\b\b{0,4:G}", "100%");
                        break;
                    }
                }
                SourceFileStream.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SourceFileStream != null)
                    SourceFileStream.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();

                if (SerFile != null)
                    SerFile.Close();
            }
        }

        /// <summary>
        /// 打包单个文件
        /// </summary>
        /// <param name="TargetFile"></param>
        /// <param name="sourceFile"></param>
        public void SerFileZip(Stream TargetStream, string sourceFile)
        {
            FileStream SourceFileStream = null;
            GZipStream CompressedStream = null;//压缩流  

            try
            {

                CompressedStream = new GZipStream(TargetStream, CompressionMode.Compress, true);

                Console.Write("打包:" + System.IO.Path.GetFileName(sourceFile) + "           ");
                SourceFileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

                long packedsize = 0;
                while (true)
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int re = SourceFileStream.Read(buffer, 0, buffer.Length);//读取源文件 
                    if (re > 0)
                    {
                        CompressedStream.Write(buffer, 0, re);//压缩 
                        packedsize += re;
                        float per = (float)packedsize / SourceFileStream.Length * 100;
                        Console.Write("\b\b\b\b{0,4:G}", per.ToString("###") + "%");
                    }
                    else
                    {
                        Console.WriteLine("\b\b\b\b{0,4:G}", "100%");
                        break;
                    }
                }
                SourceFileStream.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SourceFileStream != null)
                    SourceFileStream.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();

            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public void SerFileZip(string TargetFile, params string[] sourceFiles)
        {
            //FileStream SourceFileStream = null;
            GZipStream CompressedStream = null;//压缩流  
            FileStream SerFile = new FileStream(TargetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            try
            {
                Console.WriteLine("目标文件：" + TargetFile);
                string rootPath = System.IO.Path.GetDirectoryName(TargetFile);
                CommpressTar ctar = new CommpressTar();

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
               
                ctar.SerFilesToTar(CompressedStream, rootPath, sourceFiles);
                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (CompressedStream != null)
                    CompressedStream.Close();

                if (SerFile != null)
                    SerFile.Close();
            }
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public MemoryStream SerFileZip(string filepath)
        {
            FileStream SourceFile = null;
            GZipStream CompressedStream = null;//压缩流  
            MemoryStream SerFile = new MemoryStream();
            try
            {
                byte[] buffer;
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名
                SourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[SourceFile.Length];
                SourceFile.Read(buffer, 0, buffer.Length);//读取源文件                

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
                CompressedStream.Write(buffer, 0, buffer.Length);//压缩                
                CompressedStream.Flush();
                return SerFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SourceFile != null)
                    SourceFile.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream SerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream = new MemoryStream();
            GZipStream compressedstream = new GZipStream(zpistream, CompressionMode.Compress);//
            try
            {
                byte[] buffer = new byte[sourcestream.Length];
                sourcestream.Position = 0;
                sourcestream.Read(buffer, 0, buffer.Length);

                compressedstream.Write(buffer, 0, buffer.Length);
                compressedstream.Close();
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dirs"></param>
        public void SerDirZip(string TarFile, params string[] Dirs)
        {

            GZipStream CompressedStream = null;//压缩流  
            FileStream SerFile = new FileStream(TarFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            try
            {

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
                CommpressTar ct = new CommpressTar();
                ct.SerDirToTar(CompressedStream, Dirs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (CompressedStream != null)
                    CompressedStream.Close();

                if (SerFile != null)
                    SerFile.Close();
            }
        }
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dirs"></param>
        public void SerDirZip(string dir)
        {
            string TarFile = dir + ".tar.gz";
            SerDirZip(TarFile, dir);           
        }

        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dirs"></param>
        public void SerDirZip(string TarFile, string dir)
        {
            IO.FileHelper.DeleteFile(TarFile);
            FileStream fs = new FileStream(TarFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            GZipOutputStream gzipoutputstream = new GZipOutputStream(fs);

            try
            {
                CommpressTar ct = new CommpressTar();
                ct.SerDirToTar(gzipoutputstream, dir);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (gzipoutputstream != null)
                    gzipoutputstream.Close();

                if (fs != null)
                    fs.Close();
            }
        }

        delegate void delegateSerFile(string TargetFile, string file);
        delegate void delegateSerDir(string dir);
        delegate void delegateSerDirInP(string TargetDir, string dir);
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dirs"></param>
        public void SerDirZip(string[] Dirs)
        {
            try
            {
                delegateSerDir[] delegateSers = new delegateSerDir[Dirs.Length];
                IAsyncResult[] irs = new IAsyncResult[Dirs.Length];

                for (int i = 0; i < Dirs.Length; i++)
                {
                    delegateSers[i] = new delegateSerDir(SerDirZip);
                    irs[i] = delegateSers[i].BeginInvoke(Dirs[i], null, null);
                }
                for (int i = 0; i < Dirs.Length; i++)
                {
                    delegateSers[i].EndInvoke(irs[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dirs"></param>
        public void SerDirZipMulitThread(string TargetDir, string[] Dirs)
        {
            try
            {
                delegateSerDirInP[] delegateSers = new delegateSerDirInP[Dirs.Length];
                IAsyncResult[] irs = new IAsyncResult[Dirs.Length];
                IO.DirectoryHelper.CreateDirectory(TargetDir);
                for (int i = 0; i < Dirs.Length; i++)
                {
                    delegateSers[i] = new delegateSerDirInP(SerDirZip);
                    string tmp = System.IO.Path.Combine(TargetDir, System.IO.Path.GetFileName(Dirs[i])) + ".tar.gz";
                    irs[i] = delegateSers[i].BeginInvoke(tmp, Dirs[i], null, null);
                }
                for (int i = 0; i < Dirs.Length; i++)
                {
                    delegateSers[i].EndInvoke(irs[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public string DerFileZip(Stream sourstream, string filepath, int filelen)
        {
            FileStream SerFile = null;
            GZipStream CompressedStream = null;//压缩流  
            try
            {
                if (!System.IO.File.Exists(filepath))
                {
                    return "Err:(GZip.DerFileZIP)解压失败：指定的文件路径(" + filepath + ")不正确";
                }
                byte[] buffer = new byte[filelen];
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名

                CompressedStream = new GZipStream(sourstream, CompressionMode.Decompress, true);

                int br = CompressedStream.Read(buffer, 0, filelen);

                SerFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                SerFile.Write(buffer, 0, filelen);
                return "成功";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SerFile != null)
                    SerFile.Close();
                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream DerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream;
            sourcestream.Position = 0;
            GZipStream compressedstream = new GZipStream(sourcestream, CompressionMode.Decompress, true);//压缩流  
            try
            {
                byte[] buffer = new byte[sourcestream.Length * 100];
                int len = compressedstream.Read(buffer, 0, buffer.Length);
                compressedstream.Close();
                zpistream = new MemoryStream(buffer, 0, len);
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
    }

    public class CommpressTar
    {
        /// <summary>
        /// 打包单个文件
        /// </summary>
        /// <param name="SourFile"></param>
        /// <param name="TarFile"></param>
        public void SerFileTar(string SourFile, string TarFile)
        {
            FileStream fs = System.IO.File.Create(TarFile);
            TarOutputStream taroutputstream = new TarOutputStream(fs);
            try
            {
                FileStream ins = ins = System.IO.File.OpenRead(SourFile);

                byte[] buffer = new byte[ins.Length];

                ins.Read(buffer, 0, buffer.Length);
                ins.Close();

                string tempfile = SourFile.Substring(3, SourFile.Length - 3);
                TarEntry tarEntry = TarEntry.CreateTarEntry(tempfile);
                tarEntry.Size = buffer.Length;

                taroutputstream.PutNextEntry(tarEntry);

                taroutputstream.Write(buffer, 0, buffer.Length);
                taroutputstream.CloseEntry();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                taroutputstream.Close();
            }
        }
        /// <summary>
        /// 打包多个文件进TAR
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="files"></param>
        public void SerFilesToTar(string TarFile, string rootPath, params string[] files)
        {
            FileStream fs = new FileStream(TarFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            SerFilesToTar(fs, rootPath, files);
        }
        /// <summary>
        /// 打包多个文件进TAR
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="files"></param>
        public void SerFilesToTar(Stream TarStream, string rootPath, params string[] files)
        {

            TarOutputStream taroutputstream = new TarOutputStream(TarStream);

            try
            {
                TarHeader.EncodingName = "gb2312";
                foreach (string f in files)
                {
                    string tempfile = string.IsNullOrEmpty(rootPath) || !f.ToLower().StartsWith(rootPath.ToLower()) ? f.Substring(3, f.Length - 3) : f.Substring(rootPath.Length).Trim('\\');
                    TarHeader th = new TarHeader();

                    TarEntry tarEntry = TarEntry.CreateEntryFromFile(f);
                    tarEntry.Name = tempfile;

                    if (tarEntry.IsDirectory)
                    {
                        taroutputstream.PutNextEntry(tarEntry);
                    }
                    else
                    {
                        FileStream ins = new FileStream(f, FileMode.Open, FileAccess.Read);

                        tarEntry.Size = ins.Length;

                        taroutputstream.PutNextEntry(tarEntry);
                        //tar.WriteEntry(tarEntry, true);

                        Console.Write("打包:" + System.IO.Path.GetFileName(f) + "           ");
                        long packedsize = 0;
                        while (true)
                        {
                            if (packedsize >= tarEntry.Size) break;
                            byte[] buffer = new byte[1024 * 1024];
                            int bl = ins.Read(buffer, 0, buffer.Length);
                            if (bl > 0)
                            {
                                taroutputstream.Write(buffer, 0, bl);
                                packedsize += bl;
                                float per = (float)packedsize / ins.Length * 100;
                                Console.Write("\b\b\b\b{0,4:G}", per.ToString("###") + "%");
                            }
                            else
                            {
                                Console.WriteLine("\b\b\b\b{0,4:G}", "100%");
                                break;
                            }
                        }
                        ins.Close();
                    }
                    taroutputstream.CloseEntry();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                taroutputstream.Close();
            }
        }
        /// <summary>
        /// 将文件打包进TAR中
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="files"></param>
        public void AddFilesToTar(string TarFile, params string[] files)
        {
            FileStream fs = new FileStream(TarFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            TarOutputStream taroutputstream = new TarOutputStream(fs);
            try
            {
                TarArchive tar = TarArchive.CreateOutputTarArchive(taroutputstream);
                tar.RootPath = TarFile;
                tar.SetKeepOldFiles(true);
                TarHeader.EncodingName = "gb2312";

                Console.WriteLine("目标文件：" + TarFile);
                foreach (string f in files)
                {
                    TarEntry tarEntry = TarEntry.CreateEntryFromFile(f);

                    if (tarEntry.IsDirectory)
                    {
                        tar.WriteEntry(tarEntry, false);
                    }
                    else
                    {
                        FileStream ins = ins = System.IO.File.OpenRead(f);

                        string tempfile = f.Substring(3, f.Length - 3);

                        //tarEntry.Name = tempfile;
                        tarEntry.Size = ins.Length;

                        //taroutputstream.PutNextEntry(tarEntry);
                        tar.WriteEntry(tarEntry, false);

                        Console.Write("打包:" + System.IO.Path.GetFileName(f) + "           ");
                        long packedsize = 0;
                        while (true)
                        {
                            byte[] buffer = new byte[1024 * 1024];
                            int bl = ins.Read(buffer, 0, buffer.Length);
                            if (bl > 0)
                            {
                                taroutputstream.Write(buffer, 0, bl);
                                packedsize += bl;
                                float per = (float)packedsize / ins.Length * 100;
                                Console.Write("\b\b\b\b{0,4:G}", per.ToString("###") + "%");
                            }
                            else
                            {
                                Console.WriteLine("\b\b\b\b{0,4:G}", "100%");
                                break;
                            }
                        }
                        ins.Close();
                    }

                    taroutputstream.CloseEntry();
                }
                tar.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                taroutputstream.Close();
            }
        }

        delegate void delegateSerFileToTar(string tarfile, string rootpath, params string[] files);
        delegate void delegateSerDirToTarStream(Stream tarstream, string dir);
        delegate void delegateSerDirToTar(string tarfile, string dir);
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dir"></param>
        public void SerDirToTar(string TarFile, string Dir)
        {
            Dir = Dir.Trim('\\');
            string[] dirs = System.IO.Directory.GetDirectories(Dir, "*", SearchOption.AllDirectories);
            string[] files = System.IO.Directory.GetFiles(Dir, "*", SearchOption.AllDirectories);
            string[] alltarg = new string[dirs.Length + files.Length];
            for (int i = 0; i < alltarg.Length; i++)
            {
                if (i < dirs.Length)
                {
                    alltarg[i] = dirs[i];
                }
                else
                {
                    alltarg[i] = files[i - dirs.Length];
                }
            }
            if (string.IsNullOrEmpty(TarFile))
            {
                TarFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Dir), System.IO.Path.GetFileName(Dir) + ".tar");
            }
            SerFilesToTar(TarFile, Dir, alltarg);
        }
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dir"></param>
        public void SerDirToTar(Stream TarStream, string Dir)
        {
            Dir = Dir.Trim('\\');
            string[] dirs = System.IO.Directory.GetDirectories(Dir, "*", SearchOption.AllDirectories);
            string[] files = System.IO.Directory.GetFiles(Dir, "*", SearchOption.AllDirectories);
            string[] alltarg = new string[dirs.Length + files.Length];
            for (int i = 0; i < alltarg.Length; i++)
            {
                if (i < dirs.Length)
                {
                    alltarg[i] = dirs[i];
                }
                else
                {
                    alltarg[i] = files[i - dirs.Length];
                }
            }

            SerFilesToTar(TarStream, Dir, alltarg);
        }
        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dir"></param>
        public void SerDirToTar(params string[] dirs)
        {
            try
            {
                delegateSerDirToTar[] delegateSers = new delegateSerDirToTar[dirs.Length];
                IAsyncResult[] irs = new IAsyncResult[dirs.Length];

                for (int i = 0; i < dirs.Length; i++)
                {
                    delegateSers[i] = new delegateSerDirToTar(SerDirToTar);
                    irs[i] = delegateSers[i].BeginInvoke("", dirs[i], null, null);
                }
                for (int i = 0; i < dirs.Length; i++)
                {
                    delegateSers[i].EndInvoke(irs[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打包文件夹
        /// </summary>
        /// <param name="TarFile"></param>
        /// <param name="Dir"></param>
        public void SerDirToTar(Stream stream, params string[] dirs)
        {
            try
            {
                delegateSerDirToTarStream[] delegateSers = new delegateSerDirToTarStream[dirs.Length];
                IAsyncResult[] irs = new IAsyncResult[dirs.Length];

                for (int i = 0; i < dirs.Length; i++)
                {
                    delegateSers[i] = new delegateSerDirToTarStream(SerDirToTar);
                    irs[i] = delegateSers[i].BeginInvoke(stream, dirs[i], null, null);
                }
                for (int i = 0; i < dirs.Length; i++)
                {
                    delegateSers[i].EndInvoke(irs[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Deflate
    {
        DeflateStream CompressedStream = null;//压缩流  

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public MemoryStream SerFileZip(string filepath)
        {
            FileStream SourceFile = null;
            MemoryStream SerFile = new MemoryStream();
            try
            {
                byte[] buffer;
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名
                SourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[SourceFile.Length];
                SourceFile.Read(buffer, 0, buffer.Length);//读取源文件                

                CompressedStream = new DeflateStream(SerFile, CompressionMode.Compress, true);
                CompressedStream.Write(buffer, 0, buffer.Length);//压缩                
                CompressedStream.Flush();
                return SerFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SourceFile != null)
                    SourceFile.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream SerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream = new MemoryStream();
            DeflateStream compressedstream = new DeflateStream(zpistream, CompressionMode.Compress);//
            try
            {
                byte[] buffer = new byte[sourcestream.Length];
                sourcestream.Position = 0;
                sourcestream.Read(buffer, 0, buffer.Length);
                compressedstream.Write(buffer, 0, buffer.Length);
                compressedstream.Close();
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public void DerFileZip(Stream sourstream, string filepath, int filelen)
        {
            FileStream SerFile = null;
            try
            {
                if (!System.IO.File.Exists(filepath))
                {
                    throw new Exception("解压失败：指定的文件路径(" + filepath + ")不正确");
                }
                byte[] buffer = new byte[filelen];
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名

                CompressedStream = new DeflateStream(sourstream, CompressionMode.Decompress, true);

                int br = CompressedStream.Read(buffer, 0, filelen);

                SerFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                SerFile.Write(buffer, 0, filelen);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SerFile != null)
                    SerFile.Close();
                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream DerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream;
            sourcestream.Position = 0;
            DeflateStream compressedstream = new DeflateStream(sourcestream, CompressionMode.Decompress, true);//压缩流  
            try
            {
                byte[] buffer = new byte[sourcestream.Length * 100];
                int len = compressedstream.Read(buffer, 0, buffer.Length);
                compressedstream.Close();
                zpistream = new MemoryStream(buffer, 0, len);
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// .net中标准 的压缩方法
    /// </summary>
    public class SystemGZip
    {
        GZipStream CompressedStream = null;//压缩流          

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public MemoryStream SerFileZip(string filepath)
        {
            FileStream SourceFile = null;
            MemoryStream SerFile = new MemoryStream();
            try
            {
                //    if (!File.Exists(filepath))
                //    {
                //        return null;
                //    }
                byte[] buffer;
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名
                SourceFile = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[SourceFile.Length];
                SourceFile.Read(buffer, 0, buffer.Length);//读取源文件                

                CompressedStream = new GZipStream(SerFile, CompressionMode.Compress, true);
                CompressedStream.Write(buffer, 0, buffer.Length);//压缩                
                CompressedStream.Flush();
                return SerFile;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (SourceFile != null)
                    SourceFile.Close();

                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream SerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream = new MemoryStream();
            GZipStream compressedstream = new GZipStream(zpistream, CompressionMode.Compress);//
            try
            {
                byte[] buffer = new byte[sourcestream.Length];
                sourcestream.Position = 0;
                sourcestream.Read(buffer, 0, buffer.Length);
                compressedstream.Write(buffer, 0, buffer.Length);
                compressedstream.Close();
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="filepath">源文件</param>
        /// <param name="dir">目标目录</param>
        /// <returns></returns>
        public void DerFileZip(Stream sourstream, string filepath, int filelen)
        {
            FileStream SerFile = null;
            try
            {
                if (!System.IO.File.Exists(filepath))
                {
                    throw new Exception("解压失败：指定的文件路径(" + filepath + ")不正确");
                }
                byte[] buffer = new byte[filelen];
                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);//获取文件名

                CompressedStream = new GZipStream(sourstream, CompressionMode.Decompress, true);

                int br = CompressedStream.Read(buffer, 0, filelen);

                SerFile = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                SerFile.Write(buffer, 0, filelen);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SerFile != null)
                    SerFile.Close();
                if (CompressedStream != null)
                    CompressedStream.Close();
            }
        }
        /// <summary>
        /// 解压缩流
        /// </summary>
        /// <param name="sourcestream"></param>
        /// <returns></returns>
        public MemoryStream DerStreamZip(Stream sourcestream)
        {
            MemoryStream zpistream;
            sourcestream.Position = 0;
            GZipStream compressedstream = new GZipStream(sourcestream, CompressionMode.Decompress, true);//压缩流  
            try
            {
                byte[] buffer = new byte[sourcestream.Length * 100];
                int len = compressedstream.Read(buffer, 0, buffer.Length);
                compressedstream.Close();
                zpistream = new MemoryStream(buffer, 0, len);
                return zpistream;
            }
            catch
            {
                return null;
            }
        }
    }
}
