using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Den.Common
{
    /// <summary>
    /// HASH算法加密工具类
    /// </summary>
    public class Encryptor
    {
        /// <summary>
        /// 32位MD5算法加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="time">需要加密的次数</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor32(string str, int time)
        {
            do
            {
                str = Md5Encryptor32(str);
                time--;
            } while (time > 0);
            return str;
        }
        /// <summary>
        /// 32位MD5算法加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <param name="time">需要加密的次数</param>
        /// <param name="length">加密的长度32或16</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor32(string str, int time, int length)
        {
            do
            {
                if (length == 32)
                {
                    str = Md5Encryptor32(str);
                }
                else
                {
                    str = Md5Encryptor16(str);
                }
                time--;
            } while (time > 0);
            return str;
        }
        /// <summary>
        /// 32位MD5算法加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor32(string str)
        {
            string password = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            foreach (byte b in s)
                password += b.ToString("X2");
            return password;
        }
        /// <summary>
        /// 16位MD5算法加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5Encryptor16(string str)
        {
            string password = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            password = BitConverter.ToString(s, 4, 8).Replace("-", "");
            return password;
        }
        /// <summary>
        /// 加密原函数
        /// </summary>
        /// <param name="pToEncrypt">加密前的明文</param>
        /// <param name="sKey">Key</param>
        /// <returns>返回加密后的密文</returns>
        public static string DesEncrypt(string pToEncrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                    ret.AppendFormat("{0:X2}", b);
                cs.Close();
                cs.Dispose();
                ms.Close();
                ms.Dispose();
                return ret.ToString();
            }
            catch { return null; }
        }
        /// <summary>
        /// 解密原函数
        /// </summary>
        /// <param name="pToDecrypt">加密后的密文</param>
        /// <param name="sKey">Key</param>
        /// <returns>返回加密前的明文</returns>
        public static string DesDecrypt(string pToDecrypt, string sKey)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
                des.IV = ASCIIEncoding.ASCII.GetBytes(Md5Encryptor16(sKey).Substring(0, 8));
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                StringBuilder ret = new StringBuilder();
                byte[] obyte= ms.ToArray();
                cs.Close();
                ms.Close();
                return System.Text.Encoding.Default.GetString(obyte);
            }
            catch { return null; }
            finally
            { 

            }
        }

        public static string GetSHA1(string str)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();

            //将str转换成byte[]
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] dataToHash = enc.GetBytes(str);

            //Hash运算
            byte[] dataHashed = sha.ComputeHash(dataToHash);

            //将运算结果转换成string
            string hash = BitConverter.ToString(dataHashed).Replace("-", "");

            return hash;
        }
    }
}
