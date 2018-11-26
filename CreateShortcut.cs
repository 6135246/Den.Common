using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Den.Common
{
    /// <summary>
    /// 桌面快捷方式创建
    /// </summary>
   public class CreateShortcut
    {
        /// <summary> 
        /// 创建快捷方式 
        /// </summary> 
        /// <param name="Title">标题</param> 
        /// <param name="URL">URL地址</param> 
        /// <param name="SpecialFolder">特殊文件夹</param> 
        public static void CreateShortcutFile(string Title, string URL, string SpecialFolder)
        {
            // Create shortcut file, based on Title 
            System.IO.StreamWriter objWriter = System.IO.File.CreateText(SpecialFolder + "\\" + Title + ".url");
            // Write URL to file 
            objWriter.WriteLine("[InternetShortcut]");
            objWriter.WriteLine("URL=" + URL);
            // Close file 
            objWriter.Close();
        } 
    }

}
