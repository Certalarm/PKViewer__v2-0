using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class PublicKeyWithThreeDotConverter : Converter
    {
        private const string __threeDot = "…";

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not string publicKey)
                return string.Empty;

            var langDict = FindLangDict();
            return string.IsNullOrWhiteSpace(publicKey)
                ? langDict?[__noData] ?? string.Empty
                : MayBeAddPrefix(publicKey, p);
        }

        private static string MayBeAddPrefix(string publicKey, object p)
        {
            var result = publicKey;
            if(!result.EndsWith(__threeDot))
                result += $" {__threeDot}";
            if (p is string prefix)
                result = $"{prefix} {result}";
            return result;
        }
    }
}
