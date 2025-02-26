using PKInfo.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.UseCase.Certificates.GetInfo
{
    using DNFlags = X500DistinguishedNameFlags;
    using DN = X500DistinguishedName;

    internal static class X500DNHelper
    {
        private const string __OidWithDot = "oid.";

        internal static Dictionary<string, string> GetNeedMap(DN dnName, string[] needOIDs) =>
            GetStringDNName(dnName)
                ?.Where(x => x.Contains(Equally) && IsLineContainsNeedOIDs(x, needOIDs))
                .Select(GetPair)
                .Where(x => !string.IsNullOrWhiteSpace(x.Value))
                ?.ToDictionary(x => NormalizeOID(x.Key), x => x.Value)
                ?? [];

        private static string[] GetStringDNName(DN dnName) =>
            dnName
                ?.Decode(GetDNFlags())
                .Split([Rn], StringSplitOptions.RemoveEmptyEntries)
                ?? [];

        private static DNFlags GetDNFlags() =>
            DNFlags.DoNotUseQuotes | DNFlags.UseNewLines;

        private static bool IsLineContainsNeedOIDs(string line, string[] needOIDs)
        {
            foreach (var oid in needOIDs)
                if (IsContains(line, oid))
                    return true;
            return false;
        }

        private static KeyValuePair<string, string> GetPair(string lineWithEqually)
        {
            var parts = lineWithEqually.Split(Equally);
            return new KeyValuePair<string, string>(parts[0], lineWithEqually.Substring(parts[0].Length + 1));
        }

        private static string NormalizeOID(string stringOid) =>
            stringOid.StartsWith(__OidWithDot, StringComparison.OrdinalIgnoreCase)
                ? stringOid.Substring(__OidWithDot.Length)
                : GetOidValue(stringOid);

        private static bool IsContains(string line, string oidString)
        {
            var firstPart = line.Split(Equally)[0].Trim();
            if (firstPart.IndexOf(oidString, StringComparison.OrdinalIgnoreCase) > -1)
                return true;
            return oidString.EqualsIgnoreCase(GetOidValue(firstPart));
        }

        private static string GetOidValue(string friendlyName)
        {
            Oid oid = new(friendlyName);
            return oid.Value;
        }
    }
}
