namespace PKInfo.Utility
{
    internal static class OID
    {
        internal const string CN = "2.5.4.3"; // CommonName
        internal const string SN = "2.5.4.4"; // Surname
        internal const string G = "2.5.4.42"; // GivenName
        internal const string O = "2.5.4.10"; // Organization
        internal const string OU = "2.5.4.11"; // OrganizationUnitName
        internal const string T = "2.5.4.12"; // Title
        internal const string OGRN = "1.2.643.100.1"; // OGRN
        internal const string SNILS = "1.2.643.100.3"; // SNILS
        internal const string INNLE = "1.2.643.100.4"; // INNLE
        internal const string OGRNIP = "1.2.643.100.5"; // OGRNIP
        internal const string INN = "1.2.643.3.131.1.1"; // INN
        internal const string E = "1.2.840.113549.1.9.1"; // E-Mail

        internal static readonly string[] KnownSubjectOIDs = [ CN, SN, G, O, OU, T, OGRN, SNILS, INNLE, OGRNIP, INN, E ];
        internal static readonly string[] KnownIssuerOIDs = [ CN ];
    }
}
