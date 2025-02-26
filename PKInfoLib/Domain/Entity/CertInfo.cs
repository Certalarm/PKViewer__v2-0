using PKInfo.Utility.Enum;
using System;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.Entity
{
    public sealed class CertInfo
    {
        public string Error { get; set; } = string.Empty;
        public string Serial { get; set; }
        public DateTime? NotBeforeUTC { get; set; }
        public DateTime? NotAfterUTC { get; set; }
        public CertType Type { get; set; } = CertType.Unknown;
        public CertOwnerType OwnerType { get; set; } = CertOwnerType.Unknown;
        public string Organization { get; set; }
        public string OrganizationUnit { get; set; }
        public string Owner { get; set; }
        public string OwnerTitle { get; set; }
        public string OwnerEmail { get; set; }
        public string Issuer { get; set; }

#if DEBUG
        public override string ToString() 
            =>$"{nameof(Error)}{Colon}{Space}{Error}{Rn}"
            + $"{nameof(Serial)}{Colon}{Space}{Serial}{Rn}"
            + $"{nameof(NotBeforeUTC)}{Colon}{Space}{NotBeforeUTC?.ToString(DateFormatRu)}{Rn}"
            + $"{nameof(NotAfterUTC)}{Colon}{Space}{NotAfterUTC?.ToString(DateFormatRu)}{Rn}"
            + $"{nameof(Type)}{Colon}{Space}{Type}{Rn}"
            + $"{nameof(OwnerType)}{Colon}{Space}{OwnerType}{Rn}"
            + $"{nameof(Organization)}{Colon}{Space}{Organization}{Rn}"
            + $"{nameof(OrganizationUnit)}{Colon}{Space}{OrganizationUnit}{Rn}"
            + $"{nameof(Owner)}{Colon}{Space}{Owner}{Rn}"
            + $"{nameof(OwnerTitle)}{Colon}{Space}{OwnerTitle}{Rn}"
            + $"{nameof(OwnerEmail)}{Colon}{Space}{OwnerEmail}{Rn}"
            + $"{nameof(Issuer)}{Colon}{Space}{Issuer}"
            ;
#endif
    }
}
