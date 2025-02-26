using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using System;
using System.Globalization;
using WpfMvvm.Infrastructure.Converters.Base;
using static WpfMvvm.Infrastructure.Commands.ResourceHelper;

namespace WpfMvvm.Infrastructure.Converters
{
    public class CertAndOwnerTypeConverter : Converter
    {
        public override object Convert(object value, Type t, object p, CultureInfo c)
        {
            if (value is not CertInfo certInfo)
                return null;

            var langDict = FindLangDict();
            if (langDict == null)
                return value;

            var certType = certInfo.Type;
            var ownerType = certInfo.OwnerType;

            return IsReturnCertTypeOnly(certType, ownerType)
                ? langDict[certType.ToString()]
                : $"{langDict[certType.ToString()]}, {langDict[ownerType.ToString()]}";
        }

        private static bool IsReturnCertTypeOnly(CertType certType, CertOwnerType ownerType) =>
            certType == CertType.Unknown || certType == CertType.Unqualifed || ownerType == CertOwnerType.Unknown;
    }
}
