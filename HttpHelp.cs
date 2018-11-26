using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;

namespace Den.Common
{
    /// <summary>
    /// HTTP请求帮助类
    /// </summary>
    public class HttpHelp
    {
        //使用.NET內建機制
        public static string RemoveQueryParam2(string url, string key)
        {

            //拆出QueryString部分
            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query)) return url;
            //用HttpUtility.ParseQueryString()將其解析為成對Name/Value集合
            //在ASP.NET中直接用Request.QueryString即可
            NameValueCollection args = HttpUtility.ParseQueryString(uri.Query);
            args.Remove(key);
            return url.Substring(0, url.IndexOf("?") +
                //若有參數就多加1
                   (args.Count > 0 ? 1 : 0)) +
                   string.Join("&",
                //將NameValueCollection Cast<string>取得可LINQ的Keys集合
                               args.Cast<string>().Select(
                                   k => string.Format("{0}={1}", k,
                                       HttpUtility.UrlEncode(args[k]))));
        }

        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.UserAgent = DefaultUserAgent;
                if (!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }
                return request.GetResponse() as HttpWebResponse;
            }
            catch (Exception ex)
            {
                Den.Common.Unit.WriteErrLogToTxt("CreateGetHttpResponse", "", ex);
            }
            return null;
        }
        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        /// <param name="url">请求的URL</param>  
        /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
        /// <param name="timeout">请求的超时时间</param>  
        /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
        /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
        /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
        /// <returns></returns>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = requestEncoding.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }


        public static HttpWebResponse CreatePostHttpResponse(string url, string postData, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            if (requestEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }
            else
            {
                request.UserAgent = DefaultUserAgent;
            }

            if (timeout.HasValue)
            {
                request.Timeout = timeout.Value;
            }
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //如果需要POST数据  
            byte[] byteArray = requestEncoding.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);//写入参数
            newStream.Close();
            return request.GetResponse() as HttpWebResponse;
        }
        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            }
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
        /// <summary>
        /// 下载网络图片
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        public static void DownPic(string url, string filePath)
        {
            using (WebClient mywebclient = new WebClient())
            {
                mywebclient.DownloadFile(url, filePath);
            }
        }
        /// <summary>
        /// 微信头像和二维码下载处理
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pStrUrl"></param>
        /// <param name="pGroupID"></param>
        /// <returns></returns>
        public static string DownWxPic(int pType, string pStrUrl, string pGroupID)
        {
            string path = null;
            try
            {
                if (HttpRuntime.AppDomainAppPath != null)
                {
                    path = HttpRuntime.AppDomainAppPath + "\\DownPic\\";
                }
            }
            catch
            {
                //
            }

            if (path == null)
            {
                if (Application.StartupPath != null)
                {
                    path = Application.StartupPath + "\\DownPic\\";
                }
            }

            if (path == null)
            {
                return "";
            }
            if (!Directory.Exists(path))//如果目录不存在创建目录
            {
                Directory.CreateDirectory(path);
            }
            path = path + pGroupID + "_" + pType + ".jpg";

            using (WebClient mywebclient = new WebClient())
            {
                mywebclient.DownloadFile(pStrUrl, path);
            }
           
            string httpUrl = System.Configuration.ConfigurationManager.AppSettings["HttpRootftpUrl"];
            httpUrl += pGroupID + "/" + pGroupID + "_" + pType + ".jpg";
            FtpUpDown ftpUpDown = new FtpUpDown();
            ftpUpDown.Upload(pGroupID.ToString(), path);
            return httpUrl;
        } 

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="address">上传地址</param>
        /// <param name="fileUrl">文件http地址</param>
        /// <param name="fileName">文件名称  xxx.jpg|xxx.png</param>
        /// <returns></returns>
        public static string UpLoadFile(string address, string fileUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            request.Method = "POST";
            request.Timeout = 10000;
            var postStream = new MemoryStream();
            #region 处理Form表单文件上传
            //通过表单上传文件
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            try
            {

                var formdata = string.Format(formdataTemplate, "file", DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
                var formdataBytes = Encoding.ASCII.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                postStream.Write(formdataBytes, 0, formdataBytes.Length);

                //写入文件
                //读取文件流
                WebClient client = new WebClient();
                byte[] buffer = client.DownloadData(fileUrl);

                postStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //结尾
            var footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            #endregion

            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }
    }
}
