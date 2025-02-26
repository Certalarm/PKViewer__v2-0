using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Utility.Exts;
using static PKInfo.Utility.OID;

namespace PKInfo.Domain.UseCase.Certificates.GetInfo
{
    using StringMap = Dictionary<string, string>;

    internal static class CertTypeAndOwnerDefiner
    {
        private const int __INN_LEN = 12;
        private const int __INNLE_LEN = 10;
        private const int __SNILS_LEN = 11;
        private const int __OGRN_LEN = 13;
        private const int __OGRNIP_LEN = 15;
        private const string __TWO_ZERO = "00";

        internal static (CertType, CertOwnerType) GetTypeAndOwnerType(X509Certificate2 x509Cert)
        {
            var subjectMap = x509Cert.GetSubjectMap();
            var HasKskpExts = x509Cert.HasKskpExtensions();
            CertType certType = DefineCertType(HasKskpExts, subjectMap);
            return (certType, certType == CertType.Qualifed 
                ? DefineCertOwnerType(subjectMap) 
                : CertOwnerType.Unknown);
        }

        private static CertOwnerType DefineCertOwnerType(StringMap map)
        {
            if (IsULwoFIO(map)) return CertOwnerType.ForULwoFIO;
            if (IsUL(map)) return CertOwnerType.ForUL;
            if (IsDL(map)) return CertOwnerType.ForDL;
            if (IsIP(map)) return CertOwnerType.ForIP;
            if (IsFL(map)) return CertOwnerType.ForFL;
            return CertOwnerType.Unknown;
        }

        private static bool IsUL(StringMap map) =>
            ((IsOkINNLE(map) && IsOkINN(map)) || (!IsOkINNLE(map) && IsOkINNasINNLE(map))) 
            && IsOkOGRN(map) && IsOkSNILS(map) && map.HasKey(SN) && map.HasKey(G) && map.HasKey(T);

        private static bool IsULwoFIO(StringMap map) =>
            (IsOkINNLE(map) || IsOkINNasINNLE(map)) && IsOkOGRN(map) && !IsOkINN(map) && !IsOkSNILS(map) 
            && !map.HasKey(SN) && !map.HasKey(G) && !map.HasKey(T);

        private static bool IsDL(StringMap map) =>
            IsFL(map) && map.HasKey(T);

        private static bool IsIP(StringMap map) =>
           IsOkSNILS(map) && IsOkINN(map) && !IsOkOGRN(map) && !IsOkINNLE(map) && IsOkOGRNIP(map);

        private static bool IsFL(StringMap map) =>
            IsOkSNILS(map) && IsOkINN(map) && !IsOkOGRN(map) && !IsOkINNLE(map) && !IsOkOGRNIP(map);

        private static CertType DefineCertType(bool hasKskpExts, StringMap subjectMap) =>
            hasKskpExts && HasAnyINN(subjectMap)
                ? CertType.Qualifed
                : CertType.Unqualifed;

        private static bool IsOkOGRNIP(StringMap map) =>
            map.HasKey(OGRNIP) && IsLenEquals(map[OGRNIP], __OGRNIP_LEN);
        
        private static bool IsOkOGRN(StringMap map) =>
            map.HasKey(OGRN) && IsLenEquals(map[OGRN], __OGRN_LEN);

        private static bool IsOkSNILS(StringMap map) =>
            map.HasKey(SNILS) && IsLenEquals(map[SNILS], __SNILS_LEN);

        private static bool HasAnyINN(StringMap map) =>
            IsOkINN(map) || IsOkINNLE(map) || IsOkINNasINNLE(map);

        private static bool IsOkINNasINNLE(StringMap map) =>
            map.HasKey(INN) && IsLenEquals(map[INN], __INN_LEN) && map[INN].StartsWith(__TWO_ZERO);

        private static bool IsOkINN(StringMap map) =>
            map.HasKey(INN) && IsLenEquals(map[INN], __INN_LEN) && !map[INN].StartsWith(__TWO_ZERO);

        private static bool IsOkINNLE(StringMap map) =>
            map.HasKey(INNLE) && IsLenEquals(map[INNLE], __INNLE_LEN);

        private static bool IsLenEquals(string value, int len) => value.Length == len;
    }
}
