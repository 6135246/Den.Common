using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Den.Common
{
    /// <summary>
    /// 环信单元基础应用
    /// </summary>
    public class HXUtils
    {
        /**
         * 注册用户名册
         **/
        private static string m_orgname = "herian2015";
        public static string OrgName { get { return m_orgname; } }
        /**
         * 注册App名称
         **/
        private static string m_appname = "hzrfw";
        public static string AppName { get { return m_appname; } }
        /**
         * 客户端编码
         **/
        private static string m_client_id = "YXA6DhwRkAuDEeWyZstOxVvYZA";
        public static string Client_ID { get { return m_client_id; } }
        /**
         * 客户端secret
         **/
        private static string m_client_secret = "YXA6CYnJAmdLOUuZUF8E10O8TZACyI0";
        public static string Client_Secret { get { return m_client_secret; } }
        /**
         * 环信请求地址
         **/
        private static string m_requrlformat = "https://a1.easemob.com/{0}/{1}/";
        public static string easeMobUrl { get { return string.Format(m_requrlformat, m_orgname, m_appname); } }
        /**
         * token 验证码
         **/
        private static string m_token = "";
        public static string Token
        {
            get
            {
                if (!string.IsNullOrEmpty(m_token))
                    return m_token;
                else
                {
                    m_token = QueryToken();
                    return m_token;
                }
            }
            set { m_token = value; }
        }

        /**
         * 判断Token是否有值
         **/
        public static bool HasToken()
        {
            if (!string.IsNullOrEmpty(Token))
                return true;
            else
                return false;
        }

        /**
         * 获取环信token值
         **/
        public static string QueryToken()
        {
            if (string.IsNullOrEmpty(Client_ID) || string.IsNullOrEmpty(Client_Secret)) { return string.Empty; }

            StringBuilder _build = new StringBuilder();
            string postUrl = easeMobUrl + "token";
            string token = string.Empty;
            int expireSeconds = 0;

            _build.Append("{");
            _build.AppendFormat("\"grant_type\":\"client_credentials\",\"client_id\":\"{0}\",\"client_secret\":\"{1}\"", Client_ID, Client_Secret);
            _build.Append("}");

            string postResultStr = ReqUrl("token", "POST", _build.ToString(), string.Empty);

            JObject jo = JObject.Parse(postResultStr);
            token = jo.GetValue("access_token").ToString();
            int.TryParse(jo.GetValue("expires_in").ToString(), out expireSeconds); 
            return token;
        }

        /**
         * 环信任务请求
         **/
        public static string ReqUrl(string reqUrl, string method, string paramData, string token)
        {
            try
            {
                System.Net.HttpWebRequest request =
                        System.Net.WebRequest.Create(easeMobUrl + reqUrl) as System.Net.HttpWebRequest;
                request.Method = method.ToUpperInvariant();

                if (!string.IsNullOrEmpty(token) && token.Length > 1)
                {
                    request.Headers.Add("Authorization", "Bearer " + token);
                }

                if (request.Method.ToString() != "GET" && !string.IsNullOrEmpty(paramData) && paramData.Length > 0)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(paramData);

                    request.ContentType = "application/json";
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (System.Net.HttpWebResponse resp = request.GetResponse() as System.Net.HttpWebResponse)
                {
                    using (StreamReader stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string result = stream.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("(401)") > 0)
                {
                    Token = QueryToken();
                    return ReqUrl(reqUrl, method, paramData, Token);
                }
                else
                {  
                    throw ex;
                }
            }
        }
        


    }
}
