using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Den.Common
{
    /// <summary>
    /// 日历，中文日期库
    /// </summary>
   public class ChineseCalendarHelper
    {
       private static string ChineseNumber = "〇一二三四五六七八九";
       public const string CelestialStem = "甲乙丙丁戊己庚辛壬癸";
       public const string TerrestrialBranch = "子丑寅卯辰巳午未申酉戌亥"; 
       public static readonly string[] ChineseDayName = new string[] { "初一", "初二", "初三", "初四", "初五", "初六", "初七", "初八", "初九", "初十", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九", "二十", "廿一", "廿二", "廿三", "廿四", "廿五", "廿六", "廿七", "廿八", "廿九", "三十" }; 
       public static readonly string[] ChineseMonthName = new string[] { "正", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二" };   
       private static ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();     
       /// <summary>       
        /// 年份转换       
        /// </summary>       
        /// <param name="time"></param>       
        /// <returns></returns>       
       public static string GetYear(DateTime time)
       {
           StringBuilder sb = new StringBuilder();
           int year = calendar.GetYear(time);
           int d;
           do
           {
               d = year % 10;
               sb.Insert(0, ChineseNumber[d]);
               year = year / 10;
           }
           while (year >0);
           return sb.ToString();
       }

       /// <summary>       
        /// 月份转换       
        /// </summary>       
        /// <param name="time"></param>       
        /// <returns></returns>       
       public static string GetMonth(DateTime time)
       {
           int month = calendar.GetMonth(time);
           int year = calendar.GetYear(time);
           int leap = 0;
           for (int i = 3; i <= month; i++)
           {
               if (calendar.IsLeapMonth(year, i))
               {
                   leap = i;
                   break;//一年中最多有一个闰月
               }
           }
           if (leap > 0) month--;
           return (leap == month + 1 ? "闰" : "") + ChineseMonthName[month - 1];
       }
       /// <summary>       
       /// 月份转换阿拉伯文       
       /// </summary>       
       /// <param name="time"></param>       
       /// <returns></returns>       
       public static string GetMonthNum(DateTime time)
       {
           int month = calendar.GetMonth(time);
           int year = calendar.GetYear(time);
           int leap = 0;
           for (int i = 3; i <= month; i++)
           {
               if (calendar.IsLeapMonth(year, i))
               {
                   leap = i;
                   break;//一年中最多有一个闰月
               }
           }
           if (leap > 0) month--;
           return (leap == month + 1 ? "闰" : "") + month;
       }
       /// <summary>       
       /// 日份转换       
       /// </summary>      
       /// <param name="time"></param>      
       /// <returns></returns>       
       public static string GetDay(DateTime time)
       {
           return ChineseDayName[calendar.GetDayOfMonth(time) - 1];
       }
       /// <summary>       
       /// 天干地支       
       /// </summary>       
       /// <param name="time"></param>       
       /// <returns></returns>       
       public static string GetStemBranch(DateTime time)
       {
           int sexagenaryYear = calendar.GetSexagenaryYear(time);
           string stemBranch = CelestialStem.Substring(sexagenaryYear % 10 - 1, 1) + TerrestrialBranch.Substring(sexagenaryYear % 12 - 1, 1);
           return stemBranch;
       }
       /// <summary>
       /// 返回中文农历时间
       /// </summary>
       /// <param name="time">阳历日期</param>
       /// <param name="HasBranch">是否显示天干地支</param>
       /// <returns></returns>
       public static string GetChineseDate(DateTime time,bool HasBranch)
       {
           if (HasBranch)
           {
               return ChineseCalendarHelper.GetStemBranch(time) + ChineseCalendarHelper.GetYear(time)+"年" + ChineseCalendarHelper.GetMonth(time) +"月"+ ChineseCalendarHelper.GetDay(time)+"日";
           }
           else
           {
               return ChineseCalendarHelper.GetYear(time) + "年" + ChineseCalendarHelper.GetMonth(time) + "月" + ChineseCalendarHelper.GetDay(time) + "日";          
           }
       }
       /// <summary>
       /// 返回阿拉伯数字时间
       /// </summary>
       /// <param name="time">阳历日期</param>
       /// <param name="HasBranch">是否显示天干地支</param>
       /// <returns></returns>
       public static string GetChineseNumDate(DateTime time, bool HasBranch)
       {
           string strReturn = calendar.GetYear(time) + "年" + GetMonthNum(time) + "月" + calendar.GetDayOfMonth(time) + "日"; 
           return strReturn;

       }
    }
}
