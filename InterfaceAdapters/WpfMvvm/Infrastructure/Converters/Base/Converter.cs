using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfMvvm.Infrastructure.Converters.Base
{
    public abstract class Converter: IValueConverter
    {
        public abstract object Convert(object value, Type t, object p, CultureInfo c);

        public virtual object ConvertBack(object value, Type t, object p, CultureInfo c)
            => throw new NotSupportedException();
    }
}
