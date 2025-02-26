using PKInfo.Domain.Entity;
using System;
using System.Globalization;
using System.Windows;
using WpfMvvm.Infrastructure.Converters.Base;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;
using static WpfMvvm.Infrastructure.Converters.PluralFormHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class TimeIntervalToPluralFormConverter : Converter
    {
        private const string __pluralPartSepar = "|";

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            return value is not TimeInterval interval
                ? value
                : ToPluralForm(interval);
        }

        private static string ToPluralForm(TimeInterval interval)
        {
            var langDict = FindLangDict();
            return langDict == null
                ? string.Empty
                : GetPluralDateAndTime(langDict, interval);
        }

        private static string GetPluralDateAndTime(ResourceDictionary langDict, TimeInterval interval)
        {
            var pluralDate = GetPluralDate(langDict, interval);
            if (pluralDate == null)
                return langDict[__noData] as string;
            var pluralTime = GetPluralTime(langDict, interval);
            var pluralDateAndTime = pluralDate.Length > 0
                ? $"{pluralDate}{__comma}{__space}{pluralTime}"
                : pluralTime;
            return pluralDateAndTime.Length > 0
                ? pluralDateAndTime.TrimEnd(__space, __comma)
                : langDict[__finished] as string;
        }

        private static string GetPluralDate(ResourceDictionary langDict, TimeInterval interval)
        {
            var pluralYear = Splits(langDict[__pluralYearKey] as string);
            var pluralMonth = Splits(langDict[__pluralMonthKey] as string);
            var pluralDay = Splits(langDict[__pluralDayKey] as string);
            return GetDatePluralForm(interval, pluralYear, pluralMonth, pluralDay);
        }

        private static string GetPluralTime(ResourceDictionary langDict, TimeInterval interval)
        {
            var pluralH = langDict[__pluralHoursKey] as string;
            var pluralM = langDict[__pluralMinutesKey] as string;
            return GetTimePluralForm(interval, pluralH, pluralM);
        }

        private static string[] Splits(string values) =>
            values
                ?.Split([ __pluralPartSepar ], StringSplitOptions.RemoveEmptyEntries)
                ?? [];
    }
}
