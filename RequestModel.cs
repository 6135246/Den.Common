using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    #region 数据库操作方式
    /// <summary>
    /// 数据库操作方式
    /// </summary>
    public enum OperateAction
    {
        /// <summary>
        /// 添加
        /// </summary>
        Add = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2,
        /// <summary>
        /// 修改
        /// </summary>
        Update = 3
    }
    #endregion

    #region 数据类型公共变量类
    /// <summary>
    /// 数据类型公共变量类
    /// </summary>
    public static class DataType
    {
        /// <summary>
        /// 整数值
        /// </summary>
        public static string Int = "int";
        /// <summary>
        /// 字符串
        /// </summary>
        public static string String = "string";
        /// <summary>
        /// 日期
        /// </summary>
        public static string Datetime = "datetime";
        /// <summary>
        /// 小数、金额
        /// </summary>
        public static string Decimal = "decimal";
        /// <summary>
        /// bool值
        /// </summary>
        public static string Boolean = "bool";
        /// <summary>
        /// 表达式
        /// </summary>
        public static string Express = "express";
    }
    #endregion

    /// <summary>
    /// 返回结构(DataTable)
    /// </summary>
    public class ResultModel_List
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int flag;
        /// <summary>
        /// 消息
        /// </summary>
        public string msg;
        /// <summary>
        /// 数据集合
        /// </summary>
        public System.Data.DataTable data;
    }

    /// <summary>
    /// 返回结构(string)
    /// </summary>
    public class ResultModel_Item
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int flag;
        /// <summary>
        /// 消息
        /// </summary>
        public string msg;
        /// <summary>
        /// 数据
        /// </summary>
        public string data;
    }

    /// <summary>
    /// 增删给结构
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string table_name;
        /// <summary>
        /// 主键
        /// </summary>
        public string key;
        /// <summary>
        /// 主键值
        /// </summary>
        public string key_value;
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperateAction action;
        /// <summary>
        /// 数据集合
        /// </summary>
        public IList<Hashtable> data_item;
        /// <summary>
        /// 条件
        /// </summary>
        public string where;

        /// <summary>
        /// 添加数据集合项
        /// </summary>
        /// <param name="name">列名</param>
        /// <param name="type">数据类型</param>
        /// <param name="value">值</param>
        public void AddDataItem(string name, string type, string value)
        {
            if (data_item == null)
                data_item = new List<Hashtable>();

            Hashtable item = new Hashtable();
            item["name"] = name;
            item["type"] = type;
            item["value"] = value;

            data_item.Add(item);
        }
    }
}
