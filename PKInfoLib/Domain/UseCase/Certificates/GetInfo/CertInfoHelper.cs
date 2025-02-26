using PKInfo.Domain.Entity;
using PKInfo.Utility.Enum;
using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Domain.UseCase.Certificates.GetInfo.CertTypeAndOwnerDefiner;
using static PKInfo.Utility.OID;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.UseCase.Certificates.GetInfo
{
    using StringMap = Dictionary<string, string>;

    internal static class CertInfoHelper
    {
        internal static CertInfo GetCertInfoByX509Cert(X509Certificate2 x509Cert)
        {
            (CertType, CertOwnerType) types = GetTypeAndOwnerType(x509Cert);
            var subjectMap = x509Cert.GetSubjectMap();
            var issuerMap = x509Cert.GetIssuerMap();
            return GetCertInfo(types, subjectMap, issuerMap, x509Cert);
        }

        private static CertInfo GetCertInfo(
            (CertType, CertOwnerType) types, 
            StringMap subjectMap, 
            StringMap issuerMap, 
            X509Certificate2 x509Cert)
            => new()
                {
                    Serial = GetSerial(x509Cert),
                    NotBeforeUTC = GetNotBefore(x509Cert),
                    NotAfterUTC = GetNotAfter(x509Cert),
                    Type = types.Item1,
                    OwnerType = types.Item2,
                    Organization = GetOrganization(subjectMap),
                    OrganizationUnit = GetOrganizationUnit(subjectMap),
                    Owner = GetOwner(subjectMap),
                    OwnerTitle = GetOwnerTitle(subjectMap),
                    OwnerEmail = GetOwnerEmail(subjectMap),
                    Issuer = GetIssuer(issuerMap),
                };

        private static string GetSerial(X509Certificate2 x509Cert) =>
            x509Cert.GetSerialNumberString();

        private static DateTime GetNotBefore(X509Certificate2 x509Cert) =>
            x509Cert.NotBefore;

        private static DateTime GetNotAfter(X509Certificate2 x509Cert) =>
            x509Cert.NotAfter;

        private static string GetOrganization(StringMap map) =>
            GetValue(map, O);

        private static string GetOrganizationUnit(StringMap map) =>
            GetValue(map, OU);

        private static string GetOwnerTitle(StringMap map) =>
            GetValue(map, T);

        private static string GetOwnerEmail(StringMap map) =>
            GetValue(map, E);

        private static string GetIssuer(StringMap map) =>
            GetValue(map, CN);

        private static string GetOwner(StringMap map) =>
            map.HasKey(SN) && map.HasKey(G)
                ? $"{map[SN]}{Space}{map[G]}"
                : GetValue(map, CN);

        private static string GetValue(StringMap map, string key) =>
            map.HasKey(key)
                ? map[key]
                : null;
    }
}
