using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    /// <summary>
    /// tcpIP往服务器端发送协议
    /// </summary>
    [Serializable]   
   public class TcpIPData
    {
       /// <summary>
       /// 指令命令
        /// EXIT 退出 
        /// DUOBAOPAY 夺宝支付处理
        /// JU 获取聚币K线数据
       /// </summary>
       public string BUSI_TYPE { get; set; }
       /// <summary>
       /// 来源
       /// 0 web
       /// 1 pc客户端
       /// </summary>
       public int FROM_TYPE { get; set; }

       /// <summary>
       /// 所属集团
       /// </summary>
       public string GroupID { get; set; }
       /// <summary>
       /// 所属店铺
       /// </summary>
       public string UID { get; set; }

       /// <summary>
       /// 发送的主体内容
       /// </summary>
       public object PushMsg { get; set; }


    }
    /// <summary>
    /// tcpIP 接受协议
    /// </summary>
    [Serializable]
   public class TCPIPBackData
   {
       /// <summary>
       /// 指令命令
       /// EXIT 退出 
       /// DUOBAOPAY 夺宝支付处理
       /// </summary>
       public string BUSI_TYPE { get; set; }
       /// <summary>
       /// 错误代码 
       /// 0 正常
       /// 1 发送的数据格式不对
       /// 2 服务器未响应，请检查您的网络链接！
       /// 100 未知错误
       /// </summary>
       public int RSP_CODE { get; set; }
        
       /// <summary>
       /// 发送的主体内容
       /// </summary>
       public object PushMsg { get; set; }
   }
}
