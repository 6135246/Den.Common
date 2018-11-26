using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Collections;
using Newtonsoft.Json;
using System.Data;

namespace Den.Common
{
    /// <summary>
    /// ����Ӧ�ÿ⣬����־�����
    /// </summary>
    public class Unit
    {

        private static string DesKey = "czywfg84";                              //���ٰ�λ
        #region �ַ�����ȡ
        /// <summary>
        /// �ַ�����ȡ
        /// </summary>
        /// <param name="pstr"></param>
        /// <param name="pLen"></param>
        /// <returns></returns>
        public static string GetCutStr(string pstr, int pLen)
        {
            if (pstr.Length > pLen)
            {
                return pstr.Substring(0, pLen);
            }
            return pstr;
        }
        public static int trueLength(string str)
        {
            // str �ַ���
            // return �ַ������ֽڳ���
            int lenTotal = 0;
            int n = str.Length;
            string strWord = "";
            int asc;
            for (int i = 0; i < n; i++)
            {
                strWord = str.Substring(i, 1);
                asc = Convert.ToChar(strWord);
                if (asc < 0 || asc > 127)
                    lenTotal = lenTotal + 2;
                else
                    lenTotal = lenTotal + 1;
            }
            return lenTotal;
        }
        public static string cutTrueLength(string strOriginal, int maxTrueLength, char chrPad, bool blnCutTail)
        {

            // strOriginal ԭʼ�ַ���
            // maxTrueLength ��Ҫ���ص��ַ������ֽڳ���
            // chrPad �ַ�������ʱ������ַ�
            // blnCutTail �ַ������ֽڳ��ȳ���maxTrueLengthʱ�Ƿ�ض϶����ַ�
            // return ��������ضϺ���ַ���
            string strNew = strOriginal;

            if (strOriginal == null || maxTrueLength <= 0)
            {
                strNew = "";
                return strNew;
            }

            int trueLen = trueLength(strOriginal);
            if (trueLen > maxTrueLength)//����maxTrueLength
            {
                if (blnCutTail)//�ض�
                {
                    for (int i = strOriginal.Length - 1; i > 0; i--)
                    {
                        strNew = strNew.Substring(0, i);
                        if (trueLength(strNew) == maxTrueLength)
                            break;
                        else if (trueLength(strNew) < maxTrueLength)
                        {
                            strNew += chrPad.ToString();
                            break;
                        }
                    }
                }
            }
            else//���
            {
                for (int i = 0; i < maxTrueLength - trueLen; i++)
                {
                    strNew += chrPad.ToString();
                }
            }

            return strNew;
        }
        #endregion
        #region ʱ�䴦��
        #region ��ʱ��ת��Ϊ����
        public static string DateTimeToWeek(DateTime pDate)
        {
            switch (pDate.DayOfWeek)
            { 
                case DayOfWeek.Friday:
                    return "������";
                case DayOfWeek.Monday:
                    return "����һ";
                case DayOfWeek.Saturday:
                    return "������";
                case DayOfWeek.Sunday:
                    return "������";
                case DayOfWeek.Thursday:
                    return "������";
                case DayOfWeek.Tuesday:
                    return "���ڶ�";
                case DayOfWeek.Wednesday:
                    return "������";

            }
            return "";
        }
        #endregion

        #region ��Unixʱ���ת��ΪDateTime����ʱ��
        /// <summary>
        /// ��Unixʱ���ת��ΪDateTime����ʱ��
        /// </summary>
        /// <param name="d">13λС����������ʱ���ڲ��Զ�����</param>
        /// <returns>DateTime</returns>
        public static System.DateTime ConvertIntDateTime(long d)
        {
            System.DateTime time = System.DateTime.MinValue;
            int oCount = 13-d.ToString().Length;
            if (oCount > 1)
            {
                string oMultiplier = "";
                for (int i = 0; i < oCount; i++)
                {                   
                        oMultiplier += "0";                   
                }
                oMultiplier = "1" + oMultiplier;
                d = d * Convert.ToInt32(oMultiplier);
            }
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddMilliseconds(d);       

            return time;
        }
        #endregion

        #region ��DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
        /// <summary>
        /// ��DateTimeʱ���ʽת��ΪUnixʱ�����ʽ
        /// </summary>
        /// <param name="time">ʱ��</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeInt(System.DateTime time)
        {
            //double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0)); 
            long t = (time.Ticks - startTime.Ticks) / 10000; //��10000����Ϊ13λ
            return t;
        }
        #endregion

        /// <summary>
        /// ʱ���
        /// </summary>
        /// <param name="dateBegin">��ʼʱ��</param>
        /// <param name="dateEnd">����ʱ��</param>
        /// <param name="pFormat">0 �ܲ������ 1 ����ٷ�(Ĩ����) 2 �����ʱ(Ĩ������) 3 �������(Ĩ��ʱ����) 4 ����00:00:00��ʽ</param>
        /// <returns>����(��)��λ������: 0.00239��</returns>
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd,int pFormat)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            //����ת�ĸ�ʽ
            if (pFormat == 0)
            {
                return (ts3.Days * 24 * 60 * 60 + ts3.Hours * 60 * 60 + ts3.Minutes * 60 + ts3.Seconds).ToString();
            }
            else if (pFormat == 1)
            {
                return (ts3.Days * 24 * 60  + ts3.Hours * 60 + ts3.Minutes ).ToString();
            }
            else if (pFormat == 2)
            {
                return (ts3.Days * 24  + ts3.Hours).ToString();
            }
            else if (pFormat == 3)
            {
                return (ts3.Days * 24 + ts3.Hours).ToString();
            }
            else if (pFormat == 4)
            {
                return ts3.Days.ToString("00") + ":" + ts3.Hours.ToString("00") + ":" + ts3.Minutes.ToString("00") + ":" + ts3.Seconds.ToString("00");
            }
            return ts3.TotalMilliseconds.ToString();
        }
        #endregion

        #region ���ַ����еĵ����Ź��ˣ������Ҫ�����������ݿ���ַ�����
        /// <summary>
        /// ���ַ����еĵ����Ź��ˣ������Ҫ�����������ݿ���ַ�����
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeString(string str)
        {
            if (str == null)
            {
                return "";
            }
            return str.Replace("'", "''");
        }
        #endregion

        #region ��ȡ�����ļ��е�����
        /// <summary>
        /// ��ȡ��ϸ��Ϣ������
        /// </summary>
        /// <param name="pstrUrl"></param>
        /// <returns></returns>
        public static string ReadFromUrl(string pstrUrl)
        {
            if (pstrUrl.Trim().IndexOf("http://") == 0)
            {
                try
                {
                    System.Net.WebClient web = new System.Net.WebClient();
                    web.Encoding = System.Text.Encoding.UTF8;
                    return web.DownloadString(pstrUrl);
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return pstrUrl;
            }
        }
         

        public static string WriteHtml(string pFileName, string pContent)
        {
            string strReturn = "";
            string path = "";
            try
            {
                if (HttpRuntime.AppDomainAppPath != null)
                {
                    path = HttpRuntime.AppDomainAppPath + "\\html\\";
                    strReturn = ConfigurationManager.AppSettings["BaseUrl"].ToString()+ "/html/"+pFileName;
                }
            }
            catch
            {
                //
            }

            if (string.IsNullOrEmpty(path ))
            {
                if (Application.StartupPath != null)
                {
                    path = Application.StartupPath + "\\html\\";
                    strReturn =path+ "\\html\\"+pFileName;
                }
            }
            if (path == null)
            {
                return "";
            }
            if (!Directory.Exists(path))//���Ŀ¼�����ڴ���Ŀ¼
            {
                Directory.CreateDirectory(path);
            }
            path += pFileName;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            else
            {
                File.Delete(path);
                File.Create(path).Close();
            }
            using (StreamWriter sw = File.AppendText(path))
            {
                
                sw.Write(pContent);
                sw.Flush();
                sw.Close();
            }
            return strReturn;
        }

        //UTF8��GR2312����
        public static string UrlDeCode(string str, Encoding encoding)
        {
            if (encoding == null)
            {
                Encoding utf8 = Encoding.UTF8;
                //������utf-8���н���                    
                string code = HttpUtility.UrlDecode(str.ToUpper(), utf8);
                //���Ѿ�������ַ��ٴν��б���.
                string encode = HttpUtility.UrlEncode(code, utf8).ToUpper();
                if (str == encode)
                    encoding = Encoding.UTF8;
                else
                    encoding = Encoding.GetEncoding("gb2312");
            }
            return HttpUtility.UrlDecode(str, encoding);
        }
        #endregion

        #region ������־���浽TXT�ļ�


        /// <summary>
        /// д������־
        /// </summary>
        public static void WriteErrLogToTxt(string errTitle)
        {
            WriteErrLogToTxt("", errTitle, null);
        }

        /// <summary>
        /// д������־
        /// </summary>
        public static void WriteErrLogToTxt(string errTitle, Exception ex)
        {
            WriteErrLogToTxt("", errTitle, ex);
        }
        /// <summary>
        /// ����������
        /// </summary>
        public class ErrorDisc
        {
            public ErrorDisc(string pFileName, string pErrTitle, Exception pException)
            {
                strFileName = pFileName;
                strErrTitle = pErrTitle;
                mException = pException;

            }
            public string strFileName = "";
            public string strErrTitle = "";
            public Exception mException;
        }
        /// <summary>
        /// �����
        /// </summary>
        public static Queue<ErrorDisc> mErrorDiscList = new Queue<ErrorDisc>();
        /// <summary>
        /// ������д���ı����߳�
        /// </summary>
        public static Thread mThread;
        /// <summary>
        /// ������д���ı����߳�
        /// </summary>
        private static void WriteErr()
        {
            while (true)
            {
                if (mErrorDiscList.Count > 0)
                {
                    lock (mErrorDiscList)
                    {
                        try
                        {
                            ErrorDisc oErrorDisc = mErrorDiscList.Dequeue();
                            if (oErrorDisc != null)
                            {
                                WriteErrLogTxt(oErrorDisc.strFileName, oErrorDisc.strErrTitle, oErrorDisc.mException);
                            }
                        }
                        catch
                        { }
                    }
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// д������־
        /// </summary>
        private static void WriteErrLogTxt(string strFileName, string errTitle, Exception ex)
        {
            string path = null;
            try
            {
                if (HttpRuntime.AppDomainAppPath != null)
                {
                    path = HttpRuntime.AppDomainAppPath + "\\Error\\";
                }
            }
            catch
            {
                //
            }

            if (path == null)
            {
                if (Application.StartupPath != null)
                {
                    path = Application.StartupPath + "\\Error\\";
                }
            }

            if (path == null)
            {
                return;
            }
            if (!Directory.Exists(path))//���Ŀ¼�����ڴ���Ŀ¼
            {
                Directory.CreateDirectory(path);
            }
            path += strFileName + System.DateTime.Now.ToString("yyyyMMdd") + "Error.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            StringBuilder strBuilderErrorMessage = new StringBuilder();
            strBuilderErrorMessage.Append("________________________________________________________________________________________________________________\r\n");
            strBuilderErrorMessage.Append("����:" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            strBuilderErrorMessage.Append("�������:" + errTitle + "\r\n");
            if (ex != null)
            {
                strBuilderErrorMessage.Append("��������:" + ex.ToString() + "\r\n");
            }
            strBuilderErrorMessage.Append("________________________________________________________________________________________________________________\r\n");
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(strBuilderErrorMessage);
                sw.Flush();
                sw.Close();
            }
        }


        /// <summary>
        /// д������־
        /// </summary>
        public static void WriteErrLogToTxt(string strFileName, string errTitle, Exception ex)
        {
            ErrorDisc oErrorDisc = new ErrorDisc(strFileName, errTitle, ex);
            lock (mErrorDiscList)
            {
                mErrorDiscList.Enqueue(oErrorDisc);
            }
            if (mThread == null)
            {
                mThread = new Thread(new ThreadStart(WriteErr));
                mThread.Start();
            }
        }
        #endregion

        #region MD5����
        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="str">ԭʼ�ַ���</param>
        /// <returns>MD5���</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret.ToLower();
        }
        #endregion

        #region ���ܽ���
        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string DesEncrypt(string encryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(DesKey.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            string strReturn = Convert.ToBase64String(mStream.ToArray());

            cStream.Close();
            cStream.Dispose();

            mStream.Close();
            mStream.Dispose();
            return strReturn;
        }

        /// <summary>
        /// DES����
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DesDecrypt(string decryptString)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(DesKey.Substring(0, 8));
            byte[] keyIV = keyBytes;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        #endregion

        #region �۸���ʾ��ʱ��Ϊȥ��С�����������0
        /// <summary>
        /// �۸���ʾ��ʱ��Ϊȥ��С�����������0
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string GetDecimalForShow(decimal pDecimal)
        {
            string strDecimal = pDecimal.ToString();
            if (strDecimal.IndexOf(".") > 0)
                strDecimal = strDecimal.TrimEnd('0').TrimEnd('.');
            return strDecimal;
        }
        #endregion

        public static string NoHTML(string Htmlstring) //ȥ��HTML���
        {
            //ɾ���ű�
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }
        public static string RemoveHTML(string Htmlstring) //ȥ��HTML���
        {
            //ɾ���ű�
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //ɾ��HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", ""); 
            return Htmlstring.Trim();
        }

        #region ����Ĩβ
        /// <summary>
        /// ����Ĩβ
        /// </summary>
        /// <param name="str"></param>
        /// <param name="pLen">����λ��</param>
        /// <returns></returns>
        public static string GetNumberMoWei(string str, int pLen)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "";
                for (int i = 0; i < pLen; i++)
                {
                    if (i == 0)
                    {
                        if (str == "")
                        {
                            str = "0.0";
                        }
                    }
                    else
                    {
                        str += "0";
                    }
                }
                return str;
            }
            if (str.IndexOf('.') != -1 && str.Remove(0, str.IndexOf('.')).Length > pLen)
            {
                return str.IndexOf('.') == -1 ? str : str.Remove(str.IndexOf('.') + pLen);
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region ��ȡ��ǰʱ���
        /// <summary>  
        /// ��ȡ��ǰʱ���  
        /// </summary>  
        /// <param name="bflag">Ϊ��ʱ��ȡ10λʱ���,Ϊ��ʱ��ȡ13λʱ���.</param>  
        /// <returns></returns>  
        public static string GetTimeStamp(bool bflag = true)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }
        #endregion

        #region �Ƿ�����Ч���ֻ���
        /// <summary>
        /// �Ƿ�����Ч���ֻ���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string str)
        {
           
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            str = str.Trim();
            if (str.Length != 11)
            {
                return false;
            }
            if (!str.StartsWith("1"))
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!IsNumber(str.Substring(i, 1)))
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// �ж��Ƿ�������
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        /// <summary>
        /// �ж��ַ����Ƿ�Ϊ���������(������С��)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            return Regex.IsMatch(str, @"^[0-9]+$");
        }
        #endregion

        public static bool IsEmail(string str)
        {
            string emailStr =@"([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,5})+";
            //����������ʽ����
            Regex emailReg = new Regex(emailStr);
            if (emailReg.IsMatch(str.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Decimel���ͱ�����λλС��,����ȡ��
        /// <summary>
        /// Decimel���ͱ�����λλС��,����ȡ��
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal FormatDecimal(decimal str, int iLeng)
        {
            decimal tmp = str;
            if (tmp.ToString().IndexOf('.') > 0)
            {
                string[] arr = tmp.ToString().Split(new char[] { '.' });
                if (arr.Length == 2)
                {
                    if (arr[1].Length > iLeng)
                    {
                        tmp = Convert.ToDecimal(tmp.ToString().Substring(0, tmp.ToString().IndexOf('.') + iLeng + 1));
                    }
                }
            }
            return tmp;
        }
        #endregion

        #region ��ȡ�ַ���
        /// <summary>
        /// �ַ�������ٹ�ָ�������򽫳����Ĳ�����ָ���ַ�������
        /// </summary>
        /// <param name="p_SrcString">Ҫ�����ַ���</param>
        /// <param name="p_Length">ָ������</param>
        /// <param name="p_TailString">�����滻���ַ���</param>
        /// <returns>��ȡ����ַ���</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// �ַ�������ٹ�ָ������,��������ֱ��ɾ��
        /// </summary>
        /// <param name="p_SrcString">Ҫ�����ַ���</param>
        /// <param name="p_Length">ָ������</param>
        /// <param name="p_TailString">�����滻���ַ���</param>
        /// <returns>��ȡ����ַ���</returns>
        public static string GetSubString(string p_SrcString, int p_Length)
        {
            return GetSubString(p_SrcString, 0, p_Length, "");
        }

        /// <summary>
        /// ȡָ�����ȵ��ַ���
        /// </summary>
        /// <param name="p_SrcString">Ҫ�����ַ���</param>
        /// <param name="p_StartIndex">��ʼλ��</param>
        /// <param name="p_Length">ָ������</param>
        /// <param name="p_TailString">�����滻���ַ���</param>
        /// <returns>��ȡ����ַ���</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            if (p_Length == 0)
            {
                return p_SrcString;
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //���ַ������ȴ�����ʼλ��
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //��Ҫ��ȡ�ĳ������ַ�������Ч���ȷ�Χ��
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //��������Ч��Χ��ʱ,ֻȡ���ַ����Ľ�β

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                            {
                                nFlag = 1;
                            }
                        }
                        else
                        {
                            nFlag = 0;
                        }

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        #endregion

        #region �ַ���ת��Ϊ����
        /// <summary>
        /// ���ַ�ת��Ϊ���֣��������������ֱ�ӷ���0�����int16��short
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static short GetShort(string str)
        {
            try
            {
                return Convert.ToInt16(str);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// ���ַ�ת��Ϊ���֣��������������ֱ�ӷ���0�����int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetInt(string str)
        {
            try
            {
                if (!string.IsNullOrEmpty(str))
                {
                   
                    return Convert.ToInt32(str);
                 
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// ���ַ�ת��Ϊ���֣��������������ֱ�ӷ���0�����int32
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal GetDecimal(string str)
        {
            try
            {
                return Convert.ToDecimal(str);
            }
            catch
            {
                return 0M;
            }
        }

        /// <summary>
        /// ���ַ�ת��Ϊ���֣��������������ֱ�ӷ���0�����int64��long
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long GetLong(string str)
        {
            try
            {
                return Convert.ToInt64(str);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ��ȫ������ת��Ϊ����
        #region ȫ��ת������Լ����ת��Ϊȫ��

        /// <summary>
        /// תȫ�ǵĺ���(SBC case)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {

            // ���תȫ�ǣ�  

            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }
                if (array[i] < 127)
                {
                    array[i] = (char)(array[i] + 65248);
                }
            }
            return new string(array);
        }

        /// <summary>
        ///  ת��ǵĺ���(DBC case)  
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                {
                    array[i] = (char)(array[i] - 65248);
                }
            }
            return new string(array);
        }
        #endregion

        #endregion

        #region json����
        public static Hashtable JsonToHashtable(string strjson)
        {
            Hashtable jsonobj = (Hashtable)JsonConvert.DeserializeObject(strjson, typeof(Hashtable));
            return jsonobj;

        }
        public static DataTable JsonToDataTable(string strjson)
        {
            DataTable jsonobj = (DataTable)JsonConvert.DeserializeObject(strjson, typeof(DataTable));
            return jsonobj;

        }
        public static ArrayList JsonToArrayList(string strjson)
        {
            ArrayList jsonobj = (ArrayList)JsonConvert.DeserializeObject(strjson, typeof(ArrayList));
            return jsonobj;

        }

        /// <summary>
        /// ������ת����json����
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ConvertToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// ��Json�ַ���ת���ɶ���
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertJsonResult<T>(string json)
        {
            T result = default(T);

            result = JsonConvert.DeserializeObject<T>(json);

            return result;
        }
        /// <summary>
        /// ��Json�ַ���ת���ɶ���
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ConvertJsonResult<T>(string json, bool pUrlDecode)
        {
            if (pUrlDecode)
            {
                json = SafeUrlDecode(json);
            }
            return ConvertJsonResult<T>(json);
        }

        public static string SafeUrlDecode(string pStr)
        {
            pStr = HttpUtility.UrlDecode(pStr);
            pStr = SafeString(pStr).Replace("%2B", "+");
            return pStr;
        }
        #endregion

        #region Jeson���ݷ���
        public static Hashtable GetSendHT(string RSP_CODE, string RSP_DES, string BUSI_TYPE, object OBJECT_LIST)
        {
            Hashtable ht = new Hashtable();
            ht.Add("RSP_CODE", RSP_CODE);
            ht.Add("RSP_DES", RSP_DES);
            ht.Add("BUSI_TYPE", BUSI_TYPE);
            ht.Add("OBJECT_LIST", OBJECT_LIST);
            return ht;
        }
        #endregion

        /// <summary>
        /// ���������,ָ��������Сֵ,����ÿִ��һ�μ�һ��������λ1���Է�ֹ�ظ������
        /// </summary>
        /// <param name="MaxNumber"></param>
        /// <param name="MinNumber"></param>
        /// <returns></returns>
        public static int GetRandNumber(int MaxNumber, int MinNumber)
        {
            int _RndSeed = (int)DateTime.Now.Ticks;                  //���������
            MaxNumber = MaxNumber + 1;
            if (_RndSeed == int.MaxValue)
            {
                _RndSeed = 1;
            }
            if (MinNumber >= MaxNumber)
            {
                throw new Exception("ϵͳ����");
            }
            Random ra = new Random(_RndSeed++);
            return ra.Next(MinNumber, MaxNumber);
        }

        /// <summary>
        /// ��HTML�л�ȡ�ı�,����br,p,img
        /// </summary>
        /// <param name="HTML"></param>
        /// <returns></returns>
        public static string GetTextFromHTML(string pHTML)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return regEx.Replace(pHTML, "");
        }
    }
}
