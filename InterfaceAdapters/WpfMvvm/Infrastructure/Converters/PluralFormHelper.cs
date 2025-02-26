using PKInfo.Domain.Entity;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class PluralFormHelper
    {
        internal const char __space = ' ';
        internal const char __comma = ',';

        internal static string GetDatePluralForm(TimeInterval interval, string[] pYear, string[] pMonth, string[] pDay)
        {
            var result = string.Empty;
            int? Y = interval.Years;
            int? M = interval.Months;
            int? D = interval.Days;

            if (Y == null || M == null || D == null)
                return null;
            if (Y < 0 || M < 0 || D < 0)
                return result;

            result += GetPluralYear(Y, pYear);
            result += GetPluralMonth(M, pMonth);
            result += GetPluralDay(D, pDay);
            return result.TrimEnd(__space, __comma);
        }

        internal static string GetTimePluralForm(TimeInterval interval, string pHours, string pMinutes)
        {
            int? H = interval.Hours;
            int? M = interval.Minutes;

            if (H == null || M == null)
                return null;
            if (H < 0 || M < 0)
                return string.Empty;

            var result = GetHour(H, pHours);
            result += GetMinute(M, pMinutes);
            return result.TrimEnd(__space, __comma);
        }

        private static string GetHour(int? H, string pHours) =>
            H > 0
                ? $"{H}{__space}{AddCommaAndSpaceAfter(pHours)}"
                : string.Empty;

        private static string GetMinute(int? M, string pMinutes) =>
            M > 0
                ? $"{M}{__space}{pMinutes}"
                : string.Empty;

        private static string GetPluralYear(int? Y, string[] pYear)
        {
            var result = Y > 0
                ? $"{Y}{__space}"
                : string.Empty;
            if (IsConditionPlural0(Y))
                result += pYear[0];
            if (IsConditionPlural1(Y))
                result += pYear[1];
            if (IsConditionPlural2(Y))
                result += pYear[2];

            return result.Length > 0
                ? AddCommaAndSpaceAfter(result)
                : result;
        }

        private static bool IsConditionPlural0(int? Y) =>
            Y == 1 || (Y > 20 && Y % 10 == 1);

        private static bool IsConditionPlural1(int? Y) =>
            (Y > 1 && Y < 5) || (Y > 21 && Y % 10 > 1 && Y % 10 < 5);

        private static bool IsConditionPlural2(int? Y) =>
            (Y > 4 && Y < 21) || (Y > 21 && (Y % 10 > 4 || Y % 10 == 0));

        private static string GetPluralMonth(int? M, string[] pMonth)
        {
            var result = M > 0
                ? $"{M}{__space}"
                : string.Empty;
            return result + M switch
            {
                1 => AddCommaAndSpaceAfter(pMonth[0]),
                > 1 and < 5 => AddCommaAndSpaceAfter(pMonth[1]),
                > 4 and < 12 => AddCommaAndSpaceAfter(pMonth[2]),
                _ => result
            };
        }

        private static string GetPluralDay(int? D, string[] pDay)
        {
            var result = D > 0
                ? $"{D}{__space}"
                : string.Empty;
            return result + D switch
            {
                1 or 21 => AddCommaAndSpaceAfter(pDay[0]),
                (> 1 and < 5) or (> 21 and < 25) => AddCommaAndSpaceAfter(pDay[1]),
                (> 4 and < 21) or (> 24 and < 31) => AddCommaAndSpaceAfter(pDay[2]),
                _ => result
            };
        }

        private static string AddCommaAndSpaceAfter(string value) =>
            $"{value}{__comma}{__space}";
    }
}
