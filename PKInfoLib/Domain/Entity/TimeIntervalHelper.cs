using Microsoft.VisualBasic;
using System;

namespace PKInfo.Domain.Entity
{
    internal static class TimeIntervalHelper
    {
        internal static (int, int, int, int, int) CalcTimeInterval(DateTime startDate, DateTime endDate)
        {
            // out format = (years, month, days, hours, minutes)
            (int years, int months) = CalcYearsAndMonths(startDate, endDate);
            int days = CalcDays(startDate, endDate);
            (int hours, int minutes) = CalcHoursAndMinutes(startDate, endDate);
            return (years, months, days, hours, minutes);
        }

        private static (int, int) CalcYearsAndMonths(DateTime startDate, DateTime endDate)
        {
            int years = (int)DateAndTime.DateDiff(DateInterval.Year, startDate, endDate);
            DateTime startDateModified = startDate.AddYears(years);
            if (startDateModified > endDate)
                years--;
            int months = (int)DateAndTime.DateDiff(DateInterval.Month, startDate, endDate);
            startDateModified = startDate.AddMonths(months);
            if (startDateModified > endDate)
                months--;
            if (startDateModified > endDate && months == 0)
                months = 12;
            return (years, months % 12);
        }

        private static int CalcDays(DateTime startDate, DateTime endDate)
        {
            int daysToEndPrevMonth = GetPrevMonthDays(endDate) - startDate.Day;
            int days = startDate.Day < endDate.Day
                ? endDate.Day - startDate.Day
                : daysToEndPrevMonth + endDate.Day;
            return endDate.TimeOfDay < startDate.TimeOfDay
                ? --days
                : days;
        }

        private static int GetPrevMonthDays(DateTime date)
        {
            var dateMinusOneMonth = date.AddMonths(-1);
            return DateTime.DaysInMonth(dateMinusOneMonth.Year, dateMinusOneMonth.Month);
        }

        private static (int, int) CalcHoursAndMinutes(DateTime startTime, DateTime endTime)
        {
            int days = (int)DateAndTime.DateDiff(DateInterval.Day, startTime, endTime);
            TimeSpan hoursAndMinutes = endTime.Subtract(startTime.AddDays(days));
            int hours = hoursAndMinutes.Hours;
            int minutes = hoursAndMinutes.Minutes;
            return (hours, minutes);
        }
    }
}
