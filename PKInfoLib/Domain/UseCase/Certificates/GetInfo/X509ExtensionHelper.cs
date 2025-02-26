using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Utility.Exts;

namespace PKInfo.Domain.UseCase.Certificates.GetInfo
{
    internal static class X509ExtensionHelper
    {
        private const string __OidElSignSubject = "1.2.643.100.111";
        private const string __OidElSignIssuer = "1.2.643.100.112";

        internal static bool HasKskpExtensions(this X509Certificate2 x509cert)
        {
            var allExts = x509cert.Extensions.AsEnumerable();
            return allExts.Any()
                && HasBothKskpExts(allExts);
        }

        private static bool HasBothKskpExts(IEnumerable<X509Extension> extensions) =>
            extensions
                .Count(x => x.Oid.Value.EqualsIgnoreCase(__OidElSignSubject)
                    || x.Oid.Value.EqualsIgnoreCase(__OidElSignIssuer)) == 2;

        private static IEnumerable<X509Extension> AsEnumerable(this X509ExtensionCollection extensions)
            => extensions
                ?.OfType<X509Extension>()
                ?? [];
    }
}
