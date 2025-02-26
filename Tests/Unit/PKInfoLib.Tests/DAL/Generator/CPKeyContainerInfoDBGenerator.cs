using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.DAL.Generator
{
    internal static class CPKeyContainerInfoDBGenerator
    {
        internal static string GenNameContainer() => GenUniqueNameContainer();

        internal static DateTime GenDateNotAfter(DateTime beginDate = default, int monthOffset = 15)
        {
            var needDate = beginDate == default
                ? DefaultLocaleBeginDate
                : beginDate;
            return needDate.AddMonths(monthOffset);
        }

        internal static DateTime GenDateOfCopy(int days = 0, int minutes = 0) => BeginDateWithOffset(days, minutes);

        internal static byte[] GenPublicKey() => GenRandomBytes(8);

        internal static string GenPath(string rootPath, string containerName = default) =>
            containerName == default
                ? Path.Combine(rootPath, $"{GenRandomString(8)}.{BytesToString(GenRandomDigits(3))}")
                : Path.Combine(rootPath, containerName);

        [MethodImpl(MethodImplOptions.NoOptimization)]
        private static byte[] GenRandomBytes(int size)
        {
            byte[] bytes = new byte[size];
            GetRandom().NextBytes(bytes);
            return bytes;
        }

        private static int[] GenRandomDigits(int size)
        {
            Random rnd = GetRandom();
            int[] rndDigits = new int[size];
            for (int i = 0; i < rndDigits.Length; i++)
                rndDigits[i] = rnd.Next(0, 10);
            return rndDigits;
        }

        private static string GenRandomString(int size) =>
            Path.GetRandomFileName()
                .Replace(".", "")
                .Substring(0, size <= 11 ? size : 11);


        private static string BytesToString(int[] array) =>
           new string(array
               .SelectMany(x => x.ToString())
               .ToArray());

        private static Random GetRandom(int seed = default) =>
            seed == default
                ? new Random()
                : new Random(seed);
    }
}
