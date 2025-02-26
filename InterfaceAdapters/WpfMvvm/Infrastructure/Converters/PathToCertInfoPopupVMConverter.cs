using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using WpfMvvm.DataSource;

namespace WpfMvvm.Infrastructure.Converters
{
    public class PathToCertInfoPopupVMConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not string containerPath || containerPath == null || containerPath.Length < 1)
                return null;

            return DataModelsLoader.GetCertInfo(containerPath);
        }
    }
}
