using System;
using System.Globalization;
using System.Threading;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class CertValidityConverter : Converter
    {
        private static UtcToLocalTimeConverter _utcConverter;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value == null || value is not DateTime utcDateTime)
                return null;

            _utcConverter ??= new();
            var dtResult = _utcConverter.Convert(utcDateTime, null, null, Thread.CurrentThread.CurrentCulture);

            return dtResult as string;
        }
    }
}
