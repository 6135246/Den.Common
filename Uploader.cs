using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Web;

namespace Den.Common
{
    /// <summary>
    /// UEditor编辑器通用上传类
    /// </summary>
    public class Uploader
    {
        string state = "SUCCESS";

        string URL = null;
        string currentType = null;
        string uploadpath = null;
        string filename = null;
        string originalName = null;
        HttpPostedFile uploadFile = null;

        /**
      * 上传文件的主处理方法
      * @param HttpContext
      * @param string
      * @param  string[]
      *@param int
      * @return Hashtable
      */
        public Hashtable upFile(HttpContext cxt, string pathbase, string[] filetype, int size)
        {

            pathbase = "Supplier/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

            try
            {
                uploadFile = cxt.Request.Files[0];
                originalName = uploadFile.FileName;


                //格式验证
                if (checkType(filetype))
                {
                    state = "不允许的文件类型";
                }
                //大小验证
                if (checkSize(size))
                {
                    state = "文件大小超出网站限制";
                }
                //保存图片
                if (state == "SUCCESS")
                {
                    string strFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Path.GetExtension(uploadFile.FileName);
                    string fullpath = HttpRuntime.AppDomainAppPath + "\\UploadPhoto\\" + strFileName;//服务器文件的绝对路径
                    uploadFile.SaveAs(fullpath);


                    URL = GetBaseUrl(cxt.Request) + "/UploadPhoto/" + strFileName;

                }
            }
            catch (Exception e)
            {
                state = "未知错误";
                URL = "";
            }
            return getUploadInfo();
        }

        #region 网站URL
        public string GetBaseUrl(HttpRequest r)
        {

            string oUrl = r.Url.AbsoluteUri;
            oUrl = oUrl.Replace("http://", "");
            int oIndex = oUrl.IndexOf("/");
            if (oIndex > 0)
            {
                oUrl = oUrl.Substring(0, oIndex);
            }
            if (oUrl.Contains("localhost:"))
            {
                oUrl = "http://" + oUrl + "/Web";
            }
            else
            {
                oUrl = "http://" + oUrl;
            }
            return oUrl;
        }
        #endregion
        /**
 * 上传涂鸦的主处理方法
  * @param HttpContext
  * @param string
  * @param  string[]
  *@param string
  * @return Hashtable
 */
        public Hashtable upScrawl(HttpContext cxt, string pathbase, string tmppath, string base64Data)
        {
            pathbase = pathbase + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            uploadpath = cxt.Server.MapPath(pathbase);//获取文件上传路径
            FileStream fs = null;
            try
            {
                //创建目录
                createFolder();
                //生成图片
                filename = System.Guid.NewGuid() + ".png";
                fs = File.Create(uploadpath + filename);
                byte[] bytes = Convert.FromBase64String(base64Data);
                fs.Write(bytes, 0, bytes.Length);

                URL = pathbase + filename;
            }
            catch (Exception e)
            {
                state = "未知错误";
                URL = "";
            }
            finally
            {
                fs.Close();
                deleteFolder(cxt.Server.MapPath(tmppath));
            }
            return getUploadInfo();
        }

        /**
    * 获取文件信息
    * @param context
    * @param string
    * @return string
    */
        public string getOtherInfo(HttpContext cxt, string field)
        {
            string info = null;
            if (cxt.Request.Form[field] != null && !String.IsNullOrEmpty(cxt.Request.Form[field]))
            {
                info = field == "fileName" ? cxt.Request.Form[field].Split(',')[1] : cxt.Request.Form[field];
            }
            return info;
        }

        /**
         * 获取上传信息
         * @return Hashtable
         */
        private Hashtable getUploadInfo()
        {
            Hashtable infoList = new Hashtable();

            infoList.Add("state", state);
            infoList.Add("url", URL);

            if (currentType != null)
                infoList.Add("currentType", currentType);
            if (originalName != null)
                infoList.Add("originalName", originalName);
            return infoList;
        }

        /**
         * 重命名文件
         * @return string
         */
        private string reName()
        {
            return System.Guid.NewGuid() + getFileExt();
        }

        /**
         * 文件类型检测
         * @return bool
         */
        private bool checkType(string[] filetype)
        {
            currentType = getFileExt();
            return Array.IndexOf(filetype, currentType) == -1;
        }

        /**
         * 文件大小检测
         * @param int
         * @return bool
         */
        private bool checkSize(int size)
        {
            return uploadFile.ContentLength >= (size * 1024 * 1024);
        }

        /**
         * 获取文件扩展名
         * @return string
         */
        private string getFileExt()
        {
            string[] temp = uploadFile.FileName.Split('.');
            return "." + temp[temp.Length - 1].ToLower();
        }

        /**
         * 按照日期自动创建存储文件夹
         */
        private void createFolder()
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }

        /**
         * 删除存储文件夹
         * @param string
         */
        public void deleteFolder(string path)
        {
            //if (Directory.Exists(path))
            //{
            //    Directory.Delete(path, true);
            //}
        }
    }
}
