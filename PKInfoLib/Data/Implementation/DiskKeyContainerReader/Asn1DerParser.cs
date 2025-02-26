using System.Collections.Generic;
using System.Linq;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.Asn1DerHelper;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.Asn1DerTag;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    using ByteBoolPair = KeyValuePair<byte, bool>;
    using ByteBoolPairs = IEnumerable<KeyValuePair<byte, bool>>;
    using Bytes = IEnumerable<byte>;

    internal static class Asn1DerParser
    {
        internal static byte[] ParseContainerName(byte[] rawData) =>
            ParseBySignature(CONTAINER_NAME_SIGNATURE, rawData);

        internal static byte[] ParsePublicKey(byte[] rawData) =>
            ParseBySignature(PUBLIC_KEY_SIGNATURE, rawData);

        internal static byte[] ParseRawCert(byte[] rawData) =>
            ParseBySignature(RAW_CERT_SIGNATURE, rawData);

        internal static byte[] ParsePrivateKeyPassword(byte[] rawData) =>
            ParseBySignature(PRIVATE_KEY_PASSWORD_SIGNATURE, rawData);

        internal static byte[] ParsePrivateKeyExportable(byte[] rawData) =>
            ParseBySignature(PRIVATE_KEY_EXPORT_SIGNATURE, rawData);

        internal static byte[] ParsePrivateKeyNotAfterUTC(byte[] rawData) =>
            ParseBySignature(PRIVATE_KEY_NOT_AFTER_SIGNATURE, rawData);


        private static byte[] ParseBySignature(ByteBoolPairs signPairs, Bytes rawData)
        {
            ByteBoolPairs signPairsClone = signPairs.ToArray();
            while (signPairsClone.Any())
            {
                var pair = signPairsClone.First();
                signPairsClone = signPairsClone.Skip(1);
                var result = ParseBySignaturePair(pair, signPairsClone, rawData);
                if (result.Any()) 
                    return result;
            }
            return [];
        }

        private static byte[] ParseBySignaturePair(ByteBoolPair signPair, ByteBoolPairs signPairs, Bytes rawData)
        {
            var asn1Tag = signPair.Key;
            var isImplicit = signPair.Value;
            return isImplicit
                ? ParseIfImplicit(signPairs, rawData, asn1Tag)
                : ParseIfExplicit(signPairs, rawData, asn1Tag);
        }

        private static byte[] ParseIfImplicit(ByteBoolPairs signPairs, Bytes rawData, int asn1Tag)
        {
            if (IsBadAsn1Tag(rawData, asn1Tag))
                return [];
            var innerData = ParseInnerData(rawData.ToArray());
            return signPairs.Any()
                ? ParseBySignature(signPairs, innerData)
                : innerData;
        }

        private static byte[] ParseIfExplicit(ByteBoolPairs signPairs, Bytes rawData, int asn1Tag)
        {
            var rawDataClone = rawData.ToArray();
            while (rawDataClone.Any() && rawDataClone.First() != asn1Tag)
                rawDataClone = RemoveFirstAsnData(rawDataClone);
            if (IsBadAsn1Tag(rawDataClone, asn1Tag))
                return [];
            return signPairs.Any()
                ? ParseBySignature(signPairs, rawDataClone.Skip(CalcAsnLenSizeInBytes(rawDataClone[1])))
                : ParseInnerData(rawDataClone);
        }
    }
}
