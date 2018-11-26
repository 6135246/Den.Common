using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Den.Common
{
    /// <summary>
    /// 环信消息库
    /// </summary>
    public class HXMsg
    {
        #region 发送普通文本消息

        public static bool SentMsg(string userName, string friendsName, string msg)
        {
            try
            {
                friendsName = friendsName.Trim(' ').Trim(',').Trim(' ');
                if (string.IsNullOrEmpty(friendsName))
                {
                    return false;
                }

                StringBuilder oPara = new StringBuilder();
                oPara.Append("{");
                oPara.Append("\"target_type\":\"users\",");
                oPara.Append("\"target\":[");

                string[] friends = friendsName.Split(',');
                for (int i = 0; i < friends.Length; i++)
                {
                    oPara.AppendFormat("\"{0}\"", friends[i]);

                    if (i != friends.Length - 1)
                        oPara.Append(",");
                }

                oPara.Append("],");
                oPara.Append("\"msg\":{\"type\":\"txt\",\"msg\":\"" + msg.Replace('"', '\'') + "\"},\"from\":\"" + userName + "\"");
                oPara.Append("}");

                string result = HXUtils.ReqUrl("messages", "POST", oPara.ToString(), HXUtils.Token);

                // 往用户消息表内添加记录
                

                return true;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.SentMsg", ex);
                return false;
            }
        }

        #endregion

        #region 发送用户自定义消息

        public static bool SendUserDefinedMessage(string msg_info)
        {
            try
            {
                JObject j_info = JObject.Parse(msg_info);

                StringBuilder request_body = new StringBuilder();
                request_body.Append("{");
                request_body.AppendFormat("\"target_type\":\"{0}\"", j_info.GetValue("target_type"));
                request_body.Append(",");

                request_body.Append("\"target\":[");
                IJEnumerable<JToken> j_targets = j_info.GetValue("target").Values();
                foreach (JToken j_target in j_targets)
                {
                    request_body.AppendFormat("\"{0}\",", j_target.ToString());
                }
                request_body = request_body.Remove(request_body.Length - 1, 1);
                request_body.Append("]");
                request_body.Append(",");

                request_body.Append("\"msg\":{");
                request_body.AppendFormat("\"type\":\"{0}\"", j_info.GetValue("msgType"));
                request_body.Append(",");
                request_body.AppendFormat(
                    "\"{1}\":\"{0}\"",
                    j_info.GetValue("msg").ToString().Replace("^", "'").Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", ""),
                    j_info.GetValue("msgType").Value<string>() == "txt" ? "msg" : "action");
                request_body.Append("}");
                request_body.Append(",");

                request_body.AppendFormat("\"from\":\"{0}\"", j_info.GetValue("from"));

                if (msg_info.IndexOf("\"ext\":{") > 0)
                {
                    Dictionary<string, object> j_exts = j_info.GetValue("ext").ToObject<Dictionary<string, object>>();
                    if (j_exts.Count > 0)
                    {
                        request_body.Append(",");

                        request_body.Append("\"ext\":{");
                    }
                    foreach (KeyValuePair<string, object> j_ext in j_exts)
                    {
                        string value = "";
                        bool b_value;
                        int i_value;

                        if (bool.TryParse(j_ext.Value.ToString(), out b_value) || int.TryParse(j_ext.Value.ToString(), out i_value))
                            value = j_ext.Value.ToString();
                        else
                            value = string.Format("\"{0}\"", j_ext.Value);

                        request_body.AppendFormat("\"{0}\":{1}", j_ext.Key, value);
                        request_body.Append(",");
                    }
                    if (j_exts.Count > 0)
                    {
                        request_body = request_body.Remove(request_body.Length - 1, 1);
                        request_body.Append("}");
                    }
                }

                request_body.Append("}");
              
                string result = HXUtils.ReqUrl("messages", "POST", request_body.ToString(), HXUtils.Token);
               
                return !string.IsNullOrWhiteSpace(result) ? true : false;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.SendUserDefinedMessage", ex);
                return false;
            }
        }

        #endregion 
    }
}
