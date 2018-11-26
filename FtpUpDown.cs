using System;
using System.IO;
using System.Net;
using System.Text;
using System.Configuration;


namespace Den.Common
{
    /// <summary>
    /// FTP文件处理库
    /// </summary>
    public class FtpUpDown
    {
        private string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToString();
        private string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"].ToString();
        private string ftpUserID = ConfigurationManager.AppSettings["ftpUserID"].ToString();
        private FtpWebRequest reqFTP;

        private void Connect(string path)
        {
            this.reqFTP = (FtpWebRequest)WebRequest.Create(new Uri(path));
            this.reqFTP.UseBinary = true;
            this.reqFTP.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
        }

        public void delDir(string dirName)
        {
            try
            {
                string path = "ftp://" + this.ftpServerIP + "/" + dirName;
                this.Connect(path);
                this.reqFTP.Method = "RMD";
                ((FtpWebResponse)this.reqFTP.GetResponse()).Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void DeleteFileName(string fileName)
        {
            try
            {
                FileInfo info = new FileInfo(fileName);
                string path = "ftp://" + this.ftpServerIP + "/" + info.Name;
                this.Connect(path);
                this.reqFTP.KeepAlive = false;
                this.reqFTP.Method = "DELE";
                ((FtpWebResponse)this.reqFTP.GetResponse()).Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string Download(string LocalfilePath, string ServerfileName)
        {
            string str = "";
            try
            {
                string fileName = Path.GetFileName(ServerfileName);
                string path = LocalfilePath + @"\" + fileName;
                if (System.IO.File.Exists(path))
                {
                    str = string.Format("本地文件{0}已存在,无法下载", path);
                }
                string str4 = "ftp://" + this.ftpServerIP + "/" + ServerfileName;
                this.Connect(str4);
                this.reqFTP.Credentials = new NetworkCredential(this.ftpUserID, this.ftpPassword);
                FtpWebResponse response = (FtpWebResponse)this.reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                long contentLength = response.ContentLength;
                int count = 0x800;
                byte[] buffer = new byte[count];
                int num3 = responseStream.Read(buffer, 0, count);
                FileStream stream2 = new FileStream(path, FileMode.Create);
                while (num3 > 0)
                {
                    stream2.Write(buffer, 0, num3);
                    num3 = responseStream.Read(buffer, 0, count);
                }
                responseStream.Close();
                stream2.Close();
                response.Close();
                return "";
            }
            catch (Exception exception)
            {
                return string.Format("因{0},无法下载", exception.Message);
            }
        }

        public string[] GetFileList()
        {
            return this.GetFileList("ftp://" + this.ftpServerIP + "/", "NLST");
        }

        public string[] GetFileList(string path)
        {
            return this.GetFileList("ftp://" + this.ftpServerIP + "/" + path, "NLST");
        }

        private string[] GetFileList(string path, string WRMethods)
        {
            StringBuilder builder = new StringBuilder();
            try
            {
                this.Connect(path);
                this.reqFTP.Method = WRMethods;
                WebResponse response = this.reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
                {
                    builder.Append(str);
                    builder.Append("\n");
                }
                builder.Remove(builder.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return builder.ToString().Split(new char[] { '\n' });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string[] GetFilesDetailList()
        {
            return this.GetFileList("ftp://" + this.ftpServerIP + "/", "LIST");
        }

        public string[] GetFilesDetailList(string path)
        {
            return this.GetFileList("ftp://" + this.ftpServerIP + "/" + path, "LIST");
        }

        public long GetFileSize(string filename)
        {
            long contentLength = 0L;
            try
            {
                FileInfo info = new FileInfo(filename);
                string path = "ftp://" + this.ftpServerIP + "/" + info.Name;
                this.Connect(path);
                this.reqFTP.Method = "SIZE";
                FtpWebResponse response = (FtpWebResponse)this.reqFTP.GetResponse();
                contentLength = response.ContentLength;
                response.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return contentLength;
        }

        public void MakeDir(string dirName)
        {
            string[] strArray = dirName.Split(new char[] { '/' });
            string str = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                if (string.IsNullOrEmpty(strArray[i].ToString()))
                {
                    continue;
                }
                str = str + strArray[i].ToString() + "/";
                string path = "ftp://" + this.ftpServerIP + "/" + str;
                try
                {
                    this.Connect(path);
                    this.reqFTP.Method = "MKD";
                    ((FtpWebResponse)this.reqFTP.GetResponse()).Close();
                }
                catch(Exception ex)
                {
                    Den.Common.Unit.WriteErrLogToTxt("MakeDir", "", ex);
                }
            }
        }

        public void Rename(string currentFilename, string newFilename)
        {
            try
            {
                FileInfo info = new FileInfo(currentFilename);
                string path = "ftp://" + this.ftpServerIP + "/" + info.Name;
                this.Connect(path);
                this.reqFTP.Method = "RENAME";
                this.reqFTP.RenameTo = newFilename;
                ((FtpWebResponse)this.reqFTP.GetResponse()).Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string Upload(Stream pStream, string FolderName,string pExtname)
        {
            this.MakeDir(FolderName);
            string strFileName = Guid.NewGuid().ToString()+ pExtname;

            string path = "ftp://" + this.ftpServerIP + "/" + FolderName + "/" + strFileName;
            this.Connect(path);

            this.reqFTP.KeepAlive = false;
            this.reqFTP.Method = "STOR";
            this.reqFTP.ContentLength = pStream.Length;
            int count = 0x800;
            try
            { 
                pStream.Position = 0;
                byte[] buffer = new byte[count];
                Stream requestStream = this.reqFTP.GetRequestStream();
                for (int i = pStream.Read(buffer, 0, count); i != 0; i = pStream.Read(buffer, 0, count))
                {
                    requestStream.Write(buffer, 0, i);
                }
                requestStream.Close();
                pStream.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return strFileName;
        }
        

        public void Upload(string FolderName, string Filename)
        {
            this.MakeDir(FolderName);
            FileInfo info = new FileInfo(Filename);
            string path = "ftp://" + this.ftpServerIP + "/" + FolderName + "/" + info.Name;
            this.Connect(path);
            this.reqFTP.KeepAlive = false;
            this.reqFTP.Method = "STOR";
            this.reqFTP.ContentLength = info.Length;
            int count = 0x800;
            byte[] buffer = new byte[count];
            FileStream stream = info.OpenRead();
            try
            {
                Stream requestStream = this.reqFTP.GetRequestStream();
                for (int i = stream.Read(buffer, 0, count); i != 0; i = stream.Read(buffer, 0, count))
                {
                    requestStream.Write(buffer, 0, i);
                }
                requestStream.Close();
                stream.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
