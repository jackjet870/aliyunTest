using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aliyunTest.Framework
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取世界最小时间 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime MiniDateTime(this DateTime dt)
        {
            return new DateTime(1753, 1, 1, 12, 0, 0);
        }

        /// <summary>
        /// 获取两个日期的间隔
        /// </summary>
        /// <param name="dtStart">开始时间</param>
        /// <param name="dtEnd">结束时间</param>
        /// <param name="unit">day hour minute second millisecond</param>
        /// <returns></returns>
        public static int DateDiff(this DateTime dtStart, DateTime dtEnd, string unit)
        {
            TimeSpan tsCount = dtEnd - dtStart;
            switch (unit)
            {
                case "day":
                    return (int)tsCount.TotalDays;
                case "hour":
                    return (int)tsCount.TotalHours;
                case "minute":
                    return (int)tsCount.TotalMinutes;
                case "second":
                    return (int)tsCount.TotalSeconds;
                case "millisecond":
                    return (int)tsCount.TotalMilliseconds;
            }
            return 0;
        }

        /// <summary>
        /// 获得指定当天的时间段 如1987-4-4 00:00:00 到1987-4-4 23:59:59
        /// </summary> 
        /// <returns>如00:00:00 到23:59:59</returns>
        public static DateTime[] GetDayTimeArea(this DateTime dt)
        {
            DateTime[] d = new DateTime[2];

            DateTime temp = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);

            d[0] = temp;

            temp = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

            d[1] = temp;

            return d;
        }
        
        /// <summary>
        /// 获得指定当天的时间段的开始 如1987-4-4 00:00:00 
        /// </summary> 
        /// <returns> 如1987-4-4 00:00:00 </returns>
        public static DateTime GetDayTimeAreaStart(this DateTime dt)
        {
            return GetDayTimeArea(dt)[0];
        }

        /// <summary>
        /// 获得指定当天的时间段的结束 如1987-4-4 23:59:59 
        /// </summary> 
        /// <returns> 如1987-4-4 23:59:59  </returns>
        public static DateTime GetDayTimeAreaEnd(this DateTime dt)
        {
            return GetDayTimeArea(dt)[1];
        }

        /// <summary>
        /// 获得指定当月的时间段 如1987-4-1 00:00:00 到1987-4-30 23:59:59
        /// </summary> 
        /// <returns></returns>
        public static DateTime[] GetMonthArea(this DateTime dt)
        {
            DateTime[] d = new DateTime[2];

            DateTime temp = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0);

            d[0] = temp;

            temp = new DateTime(dt.Year, dt.Month, dt.Month == 2 ? 27 : 30, 23, 59, 59);

            d[1] = temp;

            return d;
        }

        /// <summary>
        /// 获得指定当月的开始 如1987-4-1 00:00:00 
        /// </summary> 
        /// <returns></returns>
        public static DateTime GetMonthAreaStart(this DateTime dt)
        {
            return GetMonthArea(dt)[0];
        }
        /// <summary>
        /// 获得指定当月的结束 如1987-4-30 23:59:59 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetMonthAreaEnd(this DateTime dt)
        {
            return GetMonthArea(dt)[1];
        }

    }
}
