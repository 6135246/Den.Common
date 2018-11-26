using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Data;

namespace Den.Common
{
    /// <summary>
    /// TCP/IP处理库
    /// </summary>
    public class TcpIPClient
    {

        /// <summary>
        /// 向服务器发送数据，在IsSuccess为true时，返回的object为MessageObj，否则object为一个bool值false
        /// 本方法在出错时，会自动重新请求
        /// </summary>
        /// <param name="dobj"></param>
        /// <returns></returns>
        public static TCPIPBackData TcpSend(TcpIPData dobj, string vServerIP, int vServerPort)
        {
            //加锁，当前未完成时，其他传输等待
            int failcount = 0;
            TCPIPBackData BackObj = null;
        SenderBlock:
            {
                try
                {
                    //创建Tcp客户端，准备使用NetworkStream发送数据
                    TcpClient client = new TcpClient(vServerIP, vServerPort);
                    NetworkStream ns = client.GetStream();
                    MemoryStream memStream = new MemoryStream();

                    try
                    {
                        //把要发送的数据包对象转换为字节数组
                        string value = JsonConvert.SerializeObject(dobj);
                        byte[] DataBytes = System.Text.Encoding.UTF8.GetBytes(value);

                        //数据包长度发送完后，开始发送数据包
                        ns.Write(DataBytes, 0, DataBytes.Length);

                        //读取tcp数据开始
                        byte[] buffer = new byte[1024 * 10];
                        int i = 0;

                        do
                        {
                            i = ns.Read(buffer, 0, buffer.Length);
                            memStream.Write(buffer, 0, buffer.Length);
                            buffer = new byte[1024 * 10];
                        }
                        while (ns.DataAvailable && ns.CanRead);
                        memStream.Seek(0, SeekOrigin.Begin);
                        byte[] ResultBytes = new byte[memStream.Length];
                        memStream.Read(ResultBytes, 0, ResultBytes.Length);
                        //读取tcp数据结束
                        string basepacket = System.Text.Encoding.UTF8.GetString(ResultBytes);
                        //将结果字节数据包反序列化
                        BackObj = Den.Common.Unit.ConvertJsonResult<TCPIPBackData>(basepacket);

                        //TcpIPData oTcpIPData = new TcpIPData();
                        //oTcpIPData.BUSI_TYPE = "EXIT";
                        //oTcpIPData.FROM_TYPE = dobj.FROM_TYPE;
                        //oTcpIPData.GroupID = dobj.GroupID;
                        //oTcpIPData.UID = dobj.UID;
                        //value = JsonConvert.SerializeObject(oTcpIPData);
                        //DataBytes = System.Text.Encoding.UTF8.GetBytes(value);
                        ////数据包长度发送完后，开始发送数据包
                        //ns.Write(DataBytes, 0, DataBytes.Length);

                    }
                    catch
                    {
                        goto FailedBlock;
                        //failcount++;
                        //if (failcount > 3)
                        //{
                        //    goto FailedBlock;
                        //}
                        //else
                        //{
                        //    goto SenderBlock;
                        //}
                    }
                    finally
                    {
                        //关闭相应对象
                        if (ns != null)
                        {
                            ns.Close();
                            ns.Dispose();
                        }
                        if (memStream != null)
                        {
                            memStream.Close();
                            memStream.Dispose();
                        }
                        client.Close();
                    }
                    //返回结果对象 
                    return BackObj;
                }
                catch
                {

                }
            }
        FailedBlock:
            {

                BackObj = new TCPIPBackData();
                BackObj.BUSI_TYPE = dobj.BUSI_TYPE;
                BackObj.PushMsg = "服务器未响应，请检查您的网络链接！";
                BackObj.RSP_CODE = 2;
                return BackObj;
            }
        }
    }
}
