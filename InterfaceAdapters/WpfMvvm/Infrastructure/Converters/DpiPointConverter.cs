using System;
using System.Globalization;
using System.Windows;
using WpfMvvm.Infrastructure.Converters.Base;

namespace WpfMvvm.Infrastructure.Converters
{
    public class DpiPointConverter : Converter
    {
        private const char __valuePartsDelim = ',';
        private const char __pPartsDelim = '|';

        private static DpiConverter _dpiConverter;

        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not string strValue)
                return value;
            var xy = strValue.Split([__valuePartsDelim], StringSplitOptions.RemoveEmptyEntries);
            if (xy.Length < 2)
                return value;
            return IsThickness(p)
                ? ConvertWithDpiConverter(xy[0], xy[1])
                : ConvertSegmentPoint(value, p, xy[0], xy[1]);
        }

        private object ConvertSegmentPoint(object value, object p, string x, string y)
        {
            if (p is not string strParam)
                return value;
            var pParts = strParam.Split(__pPartsDelim);
            if (pParts.Length < 2)
                return value;
            return ConvertWithDpiConverter(x, y, AddXSuffix(pParts[0]), AddYSuffix(pParts[1]));
        }

        private Point ConvertWithDpiConverter(string x, string y, string xPart = null, string yPart = null)
        {
            _dpiConverter ??= new DpiConverter();
            var numX = (double)_dpiConverter.Convert(x, null, xPart, null);
            var numY = (double)_dpiConverter.Convert(y, null, yPart, null);
            return new Point(numX, numY);
        }

        private static string AddXSuffix(string xPart) =>
            $"{xPart}_x";

        private static string AddYSuffix(string xPart) =>
            $"{xPart}_y";

        private static bool IsThickness(object p) =>
            p == null;
    }
}
