using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class DpiConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            return value is null
                ? value
                : DpiPixelTranslator.Convert(value, p);
        }
    }
}
