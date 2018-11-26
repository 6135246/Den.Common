using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Den.Common
{
    /// <summary>
    /// 用于支付回调解析用
    /// </summary>
    public class NotifyTools
    {
        public static void ReadWeiXinNotifyStream(HttpRequest req, ref SortedDictionary<string, string> sArray)
        {
            try
            {
                byte[] bytes;
                string xmlContent;
                System.IO.Stream stream;

                // 读取微支付回调请求流数据
                stream = req.InputStream;
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始 
                stream.Seek(0, System.IO.SeekOrigin.Begin);

                // 将二进制数据转换成字符串
                xmlContent = System.Text.Encoding.Default.GetString(bytes);

                // 将字符串转换成XML文档结构
                System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
                xDoc.LoadXml(xmlContent);

                // 获取XML文档的根节点
                System.Xml.XmlNode n_xml = xDoc.DocumentElement;

                // 循环取出所有底层子节点的数据，并加入字典
                foreach (System.Xml.XmlNode xNode in n_xml.ChildNodes)
                {
                    ReadXmlNodes(xNode,ref  sArray);
                }


            }
            catch (Exception) { }
        }

        private static void ReadXmlNodes(System.Xml.XmlNode xNode, ref SortedDictionary<string, string> sArray)
        {
            foreach (System.Xml.XmlNode cNode in xNode.ChildNodes)
            {
                if (cNode.HasChildNodes)
                {
                    ReadXmlNodes(cNode,ref  sArray);
                }
                else
                {
                    if (cNode.NodeType == System.Xml.XmlNodeType.CDATA || cNode.NodeType == System.Xml.XmlNodeType.Text)
                    {
                        sArray.Add(cNode.ParentNode.Name, cNode.Value);
                    }
                    else
                    {
                        sArray.Add(cNode.Name, cNode.Value);
                    }
                }
            }
        }
    }
}
