using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Den.Common
{
    /// <summary>
    /// WebAPI处理库
    /// </summary>
    public class WebApiHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        public static T Post<T>(string pUrl, string pParam, string appKeyId)
        {
            try
            {


                string strResult = PostWebUrlValue(pUrl,pParam,appKeyId);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    return default(T);
                }
                return Unit.ConvertJsonResult<T>(strResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Post请求
        /// </summary>
        public static string PostWebUrlValue(string pUrl, string pParam, string appKeyId)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(pParam);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pUrl);
                //string timeStamp = GetTimeStamp();
                //string nonce = GetNonce();
                //加入头信息
                if (!string.IsNullOrWhiteSpace(appKeyId))
                {
                    request.Headers.Add("appKeyId", appKeyId); //当前请求用户StaffId
                }
                //request.Headers.Add("timestamp", timeStamp); //发起请求时的时间戳（单位：毫秒）
                //request.Headers.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）
                //request.Headers.Add("signature", GetSignature(timeStamp, nonce, appKeyId)); //当前请求内容的数字签名
                //写数据
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);
                //读数据
                request.Timeout = 300000;
                request.Headers.Set("Cache-Control", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                string strResult = streamReader.ReadToEnd();
                //关闭流
                reqstream.Close();
                streamReader.Close();
                streamReceive.Close();
                request.Abort();
                response.Close();
                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        public static T Get<T>(string pUrl, string queryStr, string appKeyId)
        {
            try
            {

                string strResult = GetWebUrlValue(pUrl, queryStr, appKeyId);
                if (string.IsNullOrWhiteSpace(strResult))
                {
                    return default(T);
                }
                return Unit.ConvertJsonResult<T>(strResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get请求
        /// </summary>
        public static string GetWebUrlValue(string pUrl, string queryStr, string appKeyId)
        {
            try
            {
                string url = string.Empty;
                if (!string.IsNullOrEmpty(queryStr))
                {
                    url = pUrl + "?" + queryStr;
                }
                else
                {
                    url = pUrl;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //string timeStamp = GetTimeStamp();
                //string nonce = GetNonce();
                //加入头信息
                request.Headers.Add("appKeyId", appKeyId); //当前请求用户StaffId
                //request.Headers.Add("timestamp", timeStamp); //发起请求时的时间戳（单位：毫秒）
                //request.Headers.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）
                //if (sign)
                //    request.Headers.Add("signature", GetSignature(timeStamp, nonce, appKeyId)); //当前请求内容的数字签名
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 90000;
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                string strResult = streamReader.ReadToEnd();
                streamReader.Close();
                streamReceive.Close();
                request.Abort();
                response.Close();
                return strResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

 

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="staffId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSignature(string timeStamp, string nonce, string staffId)
        {
            
            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = timeStamp + nonce + staffId;
            var bytes = Encoding.UTF8.GetBytes(signStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            foreach (var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }
            return result.ToString().ToUpper();
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        /// <summary>  
        /// 获取随机数
        /// </summary>  
        /// <returns></returns>  
        public static string GetNonce()
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int i = rd.Next(0, int.MaxValue);
            return i.ToString();
        }

        /// <summary>
        /// 拼接get参数
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryString(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            // 第二步：把所有参数名和参数值串在一起
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return new Dictionary<string, string>();
            while (dem.MoveNext())
            {

                string key = dem.Current.Key;
                if (string.IsNullOrEmpty(dem.Current.Value))
                {
                    continue;
                }
                string value = dem.Current.Value.Replace("+", "%2B").Replace("&", "%26");

                if (!string.IsNullOrEmpty(key))
                {
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("queryStr", queryStr.ToString().Substring(1, queryStr.Length - 1));
            return para;
        }
    }

}
