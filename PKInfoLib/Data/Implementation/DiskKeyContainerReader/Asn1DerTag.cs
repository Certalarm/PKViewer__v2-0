using System.Collections.Generic;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal static class Asn1DerTag
    {
        private const byte BIT_STRING = 0x03;
        private const byte OCTET_STRING = 0x04;
        private const byte IA5_STRING = 0x16;
        private const byte SEQUENCE = 0x30;
        private const byte CONTEXT_SPECIFIC_1 = 0x81; 
        private const byte CONTEXT_SPECIFIC_2 = 0x82;
        private const byte CONTEXT_SPECIFIC_5 = 0x85;
        private const byte CONTEXT_SPECIFIC_10 = 0x8a;
        private const byte CONTEXT_SPECIFIC_12 = 0xac;

        internal const byte PK_EXPORTABLE_FLAG = 0xa0;
        internal const byte PK_NON_EXPORTABLE_FLAG = 0x20;

        // FORMAT PAIR: key = asn1 type, value = is Implicit

        internal static readonly KeyValuePair<byte, bool>[] CONTAINER_NAME_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(IA5_STRING, true),
            ];

        internal static readonly KeyValuePair<byte, bool>[] PUBLIC_KEY_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(CONTEXT_SPECIFIC_10, false),
            ];

        internal static readonly KeyValuePair<byte, bool>[] PRIVATE_KEY_NOT_AFTER_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(CONTEXT_SPECIFIC_12, false),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(OCTET_STRING, false),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(CONTEXT_SPECIFIC_1, true),
           ];

        internal static readonly KeyValuePair<byte, bool>[] RAW_CERT_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(CONTEXT_SPECIFIC_5, false),
            ];

        internal static readonly KeyValuePair<byte, bool>[] PRIVATE_KEY_PASSWORD_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(CONTEXT_SPECIFIC_2, false),
            ];

        internal static readonly KeyValuePair<byte, bool>[] PRIVATE_KEY_EXPORT_SIGNATURE =
            [
                new KeyValuePair<byte, bool>(SEQUENCE, true),
                new KeyValuePair<byte, bool>(SEQUENCE, false),
                new KeyValuePair<byte, bool>(SEQUENCE, false),
                new KeyValuePair<byte, bool>(BIT_STRING, true),
            ];
    }
}
