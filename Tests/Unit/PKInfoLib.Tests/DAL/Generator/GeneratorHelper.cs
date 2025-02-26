using System;

namespace PKInfoLib.Tests.DAL.Generator
{
    internal static class GeneratorHelper
    {
        private static int _NameCounter = 0;
        internal const string __TestContainerMainName = "Тестиков Тест Тестович";

        internal static readonly DateTime DefaultLocaleBeginDate = new DateTime(2024, 06, 01, 9, 0, 0); // 01 jun 2024 09:00:00 

        internal static DateTime BeginDateWithOffset(int days, int minutes = 0) => 
            DefaultLocaleBeginDate
                .AddDays(days)
                .AddMinutes(minutes);

        internal static string GenUniqueNameContainer() => $"{__TestContainerMainName} {++_NameCounter:D3}";
    }
}
