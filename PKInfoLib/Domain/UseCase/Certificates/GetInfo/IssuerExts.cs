using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Domain.UseCase.Certificates.GetInfo.X500DNHelper;
using static PKInfo.Utility.OID;

namespace PKInfo.Domain.UseCase.Certificates.GetInfo
{
    internal static class IssuerExts
    {
        public static Dictionary<string, string> GetIssuerMap(this X509Certificate2 x509Cert) =>
            GetNeedMap(x509Cert.IssuerName, KnownIssuerOIDs);
    }
}
