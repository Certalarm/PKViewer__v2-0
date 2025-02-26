using PKInfo.Domain.Entity;
using System;
using System.Globalization;
using System.Linq;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class SelfInfoConverter : Converter
    {
        private const string __dot = ".";
        private const string __zero = "0";

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            return (value is SelfInfo si)
                ? $"{si.Name} v.{VersionPart(si.Version)}{DatePart(si.ReleaseDate)}"
                : string.Empty;
        }

        private static string DatePart(DateTime dt)
        {
            var result = string.Empty;
            #if DEBUG
            result = $" [{dt:dd.MM.yyy HH:mm:ss}] - DEBUG CONFIG";
            #endif
            return result;
        }

        private static string VersionPart(string assemblyVersion)
        {
            var result = SkipFinishedZeros(assemblyVersion);
            #if DEBUG
            result = assemblyVersion;
            #endif
            return result;
        }

        private static string SkipFinishedZeros(string assemblyVersion)
        {
            var parts = assemblyVersion.Split([__dot], StringSplitOptions.RemoveEmptyEntries);
            var i = parts.Length - 1;
            while (parts[i] == __zero && i > 1)
                i--;
            return string.Join(__dot, parts.Take(i + 1));
        }

        // как определить Debug config
        //private static bool IsDebug()
        //{
        //    bool isDebug = false;
        //    #if (DEBUG)
        //    isDebug = true;
        //    #endif
        //    return isDebug;
        //}
    }
}
