using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    /// <summary>
    /// 环信用户
    /// </summary>
    public  class HxUser
    {
        #region 环信用户信息处理
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="password">密码</param>
        public static  bool AccountCreate(string userName, string password)
        {
            StringBuilder _build = new StringBuilder();
            _build.Append("{");
            _build.AppendFormat("\"username\": \"{0}\",\"password\": \"{1}\"", userName, password);
            _build.Append("}");

            try
            {
                string result = HXUtils.ReqUrl("users/" , "POST", _build.ToString(), HXUtils.Token); ;

                if (!string.IsNullOrEmpty(result))
                {
                    // 修改用户环信密码                  
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountCreate", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取指定用户是否存在
        /// </summary>
        /// <param name="userName">账号</param>
        /// <returns>会员JSON</returns>
        public static bool AccountGet(string userName)
        {
            try
            {
                string result = HXUtils.ReqUrl("users/" + userName, "GET", string.Empty, HXUtils.Token);


                if (!string.IsNullOrEmpty(result))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountGet", ex);
                return false;
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userName">账号</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>重置结果JSON(如：{ "action" : "set user password",  "timestamp" : 1404802674401,  "duration" : 90})</returns>
        public static bool AccountResetPwd(string userName, string newPassword)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    "users/" + userName + "/password",
                    "PUT",
                    "{\"newpassword\" : \"" + newPassword + "\"}",
                    HXUtils.Token
                );

                if (!string.IsNullOrEmpty(result))
                    return true;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountResetPwd", ex);
                return false;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName">账号</param>
        /// <returns>成功返回会员JSON详细信息，失败直接返回：系统错误信息</returns>
        public static bool AccountDel(string userName)
        {
            try
            {
                string result = HXUtils.ReqUrl("users/" + userName, "DELETE", string.Empty, HXUtils.Token);

                if (!string.IsNullOrEmpty(result))
                {
                    // 清除用户的环信关联账号密码和登录验证随机码
                    //ArrayList arr = new ArrayList();
                    //string strSql = string.Format("update TUser_Login set FUser_RelationID='',FUser_Serial='' where FUser_ID='{0}'", userName);
                    //arr.Add(strSql);

                    //B866MGW12.UserCenter.IDRemoting.ServerObject.GetCommonDAL().ExecuteSqlTrans(arr);

                    return true;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountResetPwd", ex);
                return false;
            }
        }

        /// <summary>
        /// 获取指定用户好友列表
        /// </summary>
        /// <param name="userName">账号</param>
        /// <returns>会员JSON</returns>
        public static bool FriendsGet(string userName)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    string.Format("users/{0}/contacts/users", userName),
                    "GET",
                    string.Empty,
                   HXUtils.Token
                );
                if (!string.IsNullOrEmpty(result))
                {
                    //JObject jo = JObject.Parse(result);
                    //string[] arr = jo.GetValue("data").ToObject<string[]>();

                    //if (arr.Length > 0)
                    //{
                    //    //B866MGW12.UserCenter.IDataView.ICommonDAL oICommonDAL = B866MGW12.UserCenter.IDRemoting.ServerObject.GetCommonDAL();
                    //    //ArrayList list = new ArrayList();

                    //    //foreach (string friend in arr)
                    //    //{
                    //    //    string oFields = " Friends_ID ";
                    //    //    string oTables = " TUser_Friends ";
                    //    //    string oWhere = string.Format(" and FUser_ID='{0}' and FUser_Friend='{1}' ", userName, friend);
                    //    //    string oOrderBy = string.Empty;
                    //    //    string oGroupBy = string.Empty;
                    //    //    DataTable dt = oICommonDAL.Query(oFields, oTables, oWhere, oOrderBy, oGroupBy);

                    //    //    if (dt == null || dt.Rows.Count == 0)
                    //    //    {
                    //    //        string strSql = string.Format("insert into TUser_Friends (Friends_ID,FUser_Id,FUser_Friend) values ('{0}','{1}','{2}')", oICommonDAL.CreateMaxID("TUser_Friends"), userName, friend);
                    //    //        list.Add(strSql);
                    //    //    }
                    //    //}

                    //    //if (list.Count > 0)
                    //    //    oICommonDAL.ExecuteSqlTrans(list);
                    //}
                }
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountResetPwd", ex);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 给指定用户添加好友
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="friendName">好友账号</param>
        public static bool FriendsAdd(string userName, string friendName)
        {
            try
            {
                if (!AccountGet(userName))
                    AccountCreate(userName, userName);

                if (!AccountGet(friendName))
                    AccountCreate(friendName, friendName);

                string result = HXUtils.ReqUrl(
                    string.Format("users/{0}/contacts/users/{1}", userName, friendName),
                    "POST",
                    string.Empty,
                    HXUtils.Token
                );


                //// 在用户好友表内，添加好友记录
                //B866MGW12.UserCenter.IDataView.ICommonDAL oICommonDAL = B866MGW12.UserCenter.IDRemoting.ServerObject.GetCommonDAL();

                //ArrayList arr = new ArrayList();
                //string strSql = string.Format("insert into TUser_Friends (Friends_ID,FUser_ID,FUser_Friend) values ('{0}','{1}','{2}')", oICommonDAL.CreateMaxID("TUser_Friends"), userName, friendName);
                //arr.Add(strSql);

                //strSql = string.Format("insert into TUser_Friends (Friends_ID,FUser_ID,FUser_Friend) values ('{0}','{1}','{2}')", oICommonDAL.CreateMaxID("TUser_Friends"), friendName, userName);
                //arr.Add(strSql);

                //oICommonDAL.ExecuteSqlTrans(arr);

                return true;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountResetPwd", ex);
                return false;
            }
        }
        /// <summary>
        /// 指定用户删除好友
        /// </summary>
        /// <param name="uesrName">用户账号</param>
        /// <param name="friendName">好友账号</param>
        public static bool FriendsDel(string userName, string friendName)
        {
            try
            {
                string result = HXUtils.ReqUrl(
                    string.Format("users/{0}/contacts/users/{1}", userName, friendName),
                    "DELETE",
                    string.Empty,
                    HXUtils.Token
                );

                // 在用户好友表内，删除好友记录
                //ArrayList arr = new ArrayList();
                //string strSql = string.Format("delete from TUser_Friends where FUser_ID='{0}' and FUser_Friend='{1}'", userName, friendName);
                //arr.Add(strSql);

                //strSql = string.Format("delete from TUser_Friends where FUser_ID='{0}' and FUser_Friend='{1}'", friendName, userName);
                //arr.Add(strSql);

                //B866MGW12.UserCenter.IDRemoting.ServerObject.GetCommonDAL().ExecuteSqlTrans(arr);

                return true;
            }
            catch (Exception ex)
            {
                Unit.WriteErrLogToTxt("hx", "hx.AccountResetPwd", ex);
                return false;
            }
        }
        #endregion
    }
}
