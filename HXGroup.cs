using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Den.Common
{
    /// <summary>
    /// 环信基础处理库
    /// </summary>
    public class HXGroup
    {
        #region 创建群组
        public static string StringFormat(object value)
        {
            return value.ToString().Replace("^", "'").Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\t", "");
        }
        public static bool CreateGroup(string group_info)
        {
            try
            {
                group_info = StringFormat(group_info);
                JObject j_info = JObject.Parse(group_info);

                StringBuilder request_body = new StringBuilder();
                request_body.Append("{");
                request_body.AppendFormat("\"groupname\":\"{0}\"", StringFormat(j_info.GetValue("groupname")));
                request_body.Append(",");
                request_body.AppendFormat("\"desc\":\"{0}\"", StringFormat(j_info.GetValue("desc")));
                request_body.Append(",");
                request_body.AppendFormat("\"public\":{0}", Convert.ToInt32(j_info.GetValue("public").ToString()) == 0 ? "true" : "false");
                request_body.Append(",");
                request_body.AppendFormat("\"maxusers\":{0}", j_info.GetValue("maxusers"));
                request_body.Append(",");
                request_body.Append("\"approval\":true");
                request_body.Append(",");
                //request_body.AppendFormat("\"membersonly\":{0}", Convert.ToInt32(j_info.GetValue("membersonly").ToString()) == 0 ? true : false);
                //request_body.Append(",");
                //request_body.AppendFormat("\"allowinvites\":{0}", Convert.ToInt32(j_info.GetValue("allowinvites").ToString()) == 0 ? true : false);
                //request_body.Append(",");
                request_body.AppendFormat("\"owner\":\"{0}\"", j_info.GetValue("owner"));
                string members = StringFormat(j_info.GetValue("members"));
                string[] member_list = members.Split(',');
                if (!string.IsNullOrWhiteSpace(members) && member_list.Length > 0)
                {
                    request_body.Append(",");
                    request_body.Append("\"members\":[");

                    for (int i = 0; i < member_list.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(member_list[i]))
                        {
                            request_body.AppendFormat("\"{0}\"", member_list[i]);

                            if (i != member_list.Length - 1)
                            {
                                request_body.Append(",");
                            }
                        }
                    }

                    request_body.Append("]");
                }

                request_body.Append("}");

                string result = HXUtils.ReqUrl("chatgroups", "POST", request_body.ToString(), HXUtils.Token);
           

                if (!string.IsNullOrEmpty(result))
                {
                    JObject j_result = JObject.Parse(result);

                    JToken j_data_token = j_result.GetValue("data");
                    if (j_data_token != null)
                    {
//                        string groupid = j_data_token.Value<string>("groupid");
//                        if (!string.IsNullOrWhiteSpace(groupid))
//                        {
//                            ArrayList arr = new ArrayList();

//                            string sql = string.Format(
//                                @"insert into TUser_HXGroups (FHXGroup_ID,FHXGroup_Name,FHXGroup_Description,FHXGroup_Public,
//                                FHXGroup_MembersOnly,FHXGroup_AllowInvites,FHXGroup_MaxUsers,FHXGroup_AffiliationsCount) 
//                                values ('{0}','{1}','{2}',{3},1,0,{4},{5})",
//                                groupid,
//                                j_info.GetValue("groupname"),
//                                j_info.GetValue("desc"),
//                                j_info.GetValue("public"),
//                                j_info.GetValue("maxusers"),
//                                string.IsNullOrWhiteSpace(members) ? 1 : member_list.Length + 1
//                            );
//                            arr.Add(sql);

//                            sql = string.Format(
//                                @"insert into TUser_HXGroupUsers (FHXGroup_ID,FUser_ID,FUser_IsOwner,FUser_IsBlocked,FHXGroup_IsBlocked,FUser_Remark) 
//                                    values ('{0}','{1}',0,1,1,'')",
//                                groupid,
//                                j_info.GetValue("owner")
//                            );
//                            arr.Add(sql);

//                            if (!string.IsNullOrWhiteSpace(members) && member_list.Length > 0)
//                            {
//                                foreach (string member in member_list)
//                                {
//                                    sql = string.Format(
//                                        @"insert into TUser_HXGroupUsers (FHXGroup_ID,FUser_ID,FUser_IsOwner,FUser_IsBlocked,FHXGroup_IsBlocked,FUser_Remark) 
//                                    values ('{0}','{1}',1,1,1,'')",
//                                        groupid,
//                                        member
//                                    );
//                                    arr.Add(sql);
//                                }
//                            }

//                            B866MGW12.UserCenter.IDRemoting.ServerObject.GetCommonDAL().ExecuteSqlTrans(arr);
//                        }
                    }
                   
                    return true;
                }
                else
                {
                     
                    return false;
                }
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.ExitGroup", ex);
                return false;
            }
        }

        #endregion

        #region 修改群组信息

        public static bool ModifyGroup(string group_info)
        {
            try
            {
                JObject j_info = JObject.Parse(group_info);
                JToken j_value = null;

                StringBuilder request_body = new StringBuilder();
                request_body.Append("{");

                if (j_info.TryGetValue("groupname", out j_value) && j_value != null)
                {
                    request_body.AppendFormat("\"groupname\":\"{0}\"", j_value);
                }

                if (j_info.TryGetValue("description", out j_value) && j_value != null)
                {
                    if (request_body.Length > 1)
                        request_body.Append(",");

                    request_body.AppendFormat("\"description\":\"{0}\"", j_value);
                }

                if (j_info.TryGetValue("maxusers", out j_value) && j_value != null)
                {
                    if (request_body.Length > 1)
                        request_body.Append(",");

                    request_body.AppendFormat("\"maxusers\":{0}", j_value);
                }

                request_body.Append("}");

                string result = HXUtils.ReqUrl(
                    "chatgroups/" + j_info.GetValue("groupid").ToString(),
                    "PUT",
                    request_body.ToString(),
                    HXUtils.Token
                );
              

                return !string.IsNullOrWhiteSpace(result) ? true : false;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.ExitGroup", ex);
                return false;
            }
        }

        #endregion

        #region 加入群组

        public static bool JoinGroup(string groupid, string userid)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    string.Format("chatgroups/{0}/users/{1}", groupid, userid),
                    "POST",
                    string.Empty,
                    HXUtils.Token
                );

              
                if (!string.IsNullOrWhiteSpace(result))
                {
                    JObject j_result = JObject.Parse(result);

                    return Convert.ToBoolean(j_result.GetValue("data").Value<string>("result"));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.ExitGroup", ex);
                return false;
            }
        }

        #endregion

        #region 群组减人

        public static bool ExitGroup(string groupid, string userid)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    string.Format("chatgroups/{0}/users/{1}", groupid, userid),
                    "DELETE",
                    string.Empty,
                    HXUtils.Token
                );

               
                if (!string.IsNullOrWhiteSpace(result))
                {
                    JObject j_result = JObject.Parse(result);

                    return Convert.ToBoolean(j_result.GetValue("data").Value<string>("result"));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.ExitGroup", ex);
                return false;
            }
        }

        #endregion

        #region 解散群组

        public static bool DeleteGroup(string groupid)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    string.Format("chatgroups/{0}", groupid),
                    "DELETE",
                    string.Empty,
                    HXUtils.Token
                );

               // Log.LogInstance.WriteLog("delete group=======" + result);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    JObject j_result = JObject.Parse(result);

                    return Convert.ToBoolean(j_result.GetValue("data").Value<string>("success"));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.DeleteGroup", ex);
                return false;
            }
        }

        #endregion 
    }
}
