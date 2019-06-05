//////////////////////////////////////////////////
// Author   : jiamao
// Date     : 2010/09/15
// Usage    : 日志操作类
//////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;

namespace DoNet.Common.Data
{
    /// <summary>
    /// 日期处理
    /// </summary>
    public class DateHelper
    {
        ///   <summary>   
        ///   取指定日期是一年中的第几周   
        ///   </summary>   
        ///   <param   name="dtime">给定的日期</param>   
        ///   <returns>数字   一年中的第几周</returns>   
        public static int WeekOfYear(DateTime dtime)
        {
            int firstdayofweek = System.Convert.ToDateTime(dtime.Year.ToString() + "- " + "1-1 ").DayOfWeek.GetHashCode();
            int days = dtime.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);
            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                {
                    weeks++;
                }
                return weeks + 1;
            }
        }

        /// <summary>
        /// 获取日期对应的星期
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static string GetWeekDay(DateTime dtime)
        {
            switch (dtime.DayOfWeek)
            {
                case DayOfWeek.Friday: return "星期五";
                case DayOfWeek.Monday: return "星期一";
                case DayOfWeek.Saturday: return "星期六";
                case DayOfWeek.Sunday: return "星期日";
                case DayOfWeek.Thursday: return "星期四";
                case DayOfWeek.Tuesday: return "星期二";
                case DayOfWeek.Wednesday: return "星期三";
                default: return "未知";
            }
        }

        /// <summary>
        /// 获取日期对应的星期
        /// </summary>
        /// <param name="dtime"></param>
        /// <returns></returns>
        public static int GetWeekIntDay(DateTime dtime)
        {
            switch (dtime.DayOfWeek)
            {
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 7;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                default: return 0;
            }
        }
    }
}
