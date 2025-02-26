using System;
using System.Globalization;
using System.Linq;

namespace WpfMvvm.Infrastructure.Converters
{
    internal static class DpiPixelTranslator
    {
        private const char __pPartsDelim = '_';
        private const string __xCoordMarker = "x";
        private const string __horMarker = "h";
        private const string __beginMarker = "b";
        private const double __etalonDpi = 96.0;
        private const double __halfStrokeThickness = 1.0; // full stroke thickness = 2.0
        private static readonly double __dpiX = System.Windows.Media.VisualTreeHelper
                .GetDpi(new System.Windows.Controls.Control()).PixelsPerInchX;
        private static readonly double __ratio = __dpiX / __etalonDpi;
        private static readonly NumberFormatInfo __numFormat = new()
        {
            NumberDecimalSeparator = ".",
        };

        private static double _valueInWpfPx;    // value in wpf virtual px
        private static double _pathSizeInWpfPx; // path size in wpf virtual px
        private static bool _isXcoord;          // 'true': X-coord, 'false': Y-coord
        private static bool _isHor;             // 'true': horizontal, 'false': vertical
        private static bool _isFromBegin;       // 'true': draw from begin, 'false': draw from end

        internal static double Convert(object value, object p)
        {
            _valueInWpfPx = Double.Parse((string)value, __numFormat);
            return IsThickness(p)
                ? CalcThickness()
                : CalcSegmentCoord(p);
        }

        private static bool IsThickness(object p) => p == null;

        private static double CalcThickness() =>
            (_valueInWpfPx * 1000.0 / __ratio) / 1000.0;

        private static double CalcSegmentCoord(object p)
        {
            if (p is not string strP)
                return CalcThickness();
            var pParts = strP.Split(__pPartsDelim);
            if (pParts.Length < 4)
                return CalcThickness();
            InitFieldsByP(pParts);
            return CalcCoordBySwitch();
        }

        private static void InitFieldsByP(string[] pParts)
        {
            _isXcoord = IsXcoord(pParts);
            _isHor = IsHor(pParts);
            _isFromBegin = IsBegin(pParts);
            _pathSizeInWpfPx = double.Parse(pParts[0]);
        }

        private static double CalcCoordBySwitch() =>
            (_isXcoord, _isHor, _isFromBegin) switch
            {
                (true, true, true) or
                (false, false, true) => CalcXHorOrYVertBeginCoord(),
                (true, true, false) or
                (false, false, false) => CalcXHorOrYVertEndCoord(),
                (true, false, true) or
                (false, true, true) => CalcXVertOrYHorBeginCoord(),
                (true, false, false) or
                (false, true, false) => CalcXVertOrYHorEndCoord(),
            };

        private static double CalcXHorOrYVertBeginCoord() =>
            RoundValueInPhysPx() / __ratio;

        private static double CalcXHorOrYVertEndCoord() =>
            (RoundPathSizeInPhysPx() - RoundValueInPhysPx()) / __ratio;

        private static double CalcXVertOrYHorBeginCoord() =>
            (RoundValueInPhysPx() + __halfStrokeThickness) / __ratio;

        private static double CalcXVertOrYHorEndCoord() =>
            (RoundPathSizeInPhysPx() - (RoundValueInPhysPx() + __halfStrokeThickness)) / __ratio;

        private static double RoundValueInPhysPx() => RoundToPhysPx(_valueInWpfPx * __ratio);

        private static double RoundPathSizeInPhysPx() => RoundToPhysPx(_pathSizeInWpfPx * __ratio);

        private static double RoundToPhysPx(double value)
        {
            double fractPart = GetFractPart(value);
            var roundPix = RoundFractPartToPhysPx(fractPart);
            return value - fractPart + roundPix;
        }

        private static double GetFractPart(double value) =>
            value - Math.Truncate(value);

        private static bool IsXcoord(string[] pParts) =>
            IsArrayContains(pParts, __xCoordMarker);

        private static bool IsHor(string[] pParts) =>
            IsArrayContains(pParts, __horMarker);

        private static bool IsBegin(string[] pParts) =>
            IsArrayContains(pParts, __beginMarker);

        private static bool IsArrayContains(string[] pParts, string value) =>
            pParts.Contains(value);

        private static double RoundFractPartToPhysPx(double value) =>
            value <= 0.5
                ? 0.0
                : 1.0;
    }
}
