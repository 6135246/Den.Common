using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    /// <summary>
    /// Http消息接口处理库
    /// </summary>
    public class ResultMsg
    {
        public ResultMsg(){}

        public object Data { get; set; }
        public string Info { get; set; }
        public int StatusCode { get; set; }
    }
   
    public enum StatusCodeEnum
    {
        [Text("请求(或处理)成功")]
        Success = 200,
        [Text("请求参数不完整或不正确")]
        ParameterError = 400,
        [Text("未授权标识")]
        Unauthorized = 401,
        [Text("请求TOKEN失效")]
        TokenInvalid = 403,
        [Text("HTTP请求类型不合法")]
        HttpMehtodError = 405,
        [Text("HTTP请求不合法,请求参数可能被篡改")]
        HttpRequestError = 406,
        [Text("该URL已经失效")]
        URLExpireError = 407,
        [Text("内部请求出错")]
        Error = 500,
        [Text("请求(或处理)失败")]
        Fail = 505,
    }
  
    public class TextAttribute : Attribute
    {
        public TextAttribute(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
    public static class EnumExtension
    {
        private static Dictionary<string, Dictionary<string, string>> enumCache;

        private static Dictionary<string, Dictionary<string, string>> EnumCache
        {
            get
            {
                if (enumCache == null)
                {
                    enumCache = new Dictionary<string, Dictionary<string, string>>();
                }
                return enumCache;
            }
            set { enumCache = value; }
        }

        public static string GetEnumText(this Enum en)
        {
            string enString = string.Empty;
            if (null == en) return enString;
            var type = en.GetType();
            enString = en.ToString();
            if (!EnumCache.ContainsKey(type.FullName))
            {
                var fields = type.GetFields();
                Dictionary<string, string> temp = new Dictionary<string, string>();
                foreach (var item in fields)
                {
                    var attrs = item.GetCustomAttributes(typeof(TextAttribute), false);
                    if (attrs.Length == 1)
                    {
                        var v = ((TextAttribute)attrs[0]).Value;
                        temp.Add(item.Name, v);
                    }
                }
                EnumCache.Add(type.FullName, temp);
            }
            if (EnumCache[type.FullName].ContainsKey(enString))
            {
                return EnumCache[type.FullName][enString];
            }
            return enString;
        }
    }
}
