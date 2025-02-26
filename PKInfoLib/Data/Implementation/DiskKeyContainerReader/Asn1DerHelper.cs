using System.Collections.Generic;
using System.Linq;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal static class Asn1DerHelper
    {
        internal static byte[] ParseInnerData(byte[] data) =>
            data
                .Skip(CalcAsnLenSizeInBytes(data[1]))
                .Take(CalcAsnLenInBytes(data))
                .ToArray();

        internal static bool IsBadAsn1Tag(IEnumerable<byte> data, int asn1Tag) =>
           !data.Any() || data.First() != asn1Tag;

        internal static byte[] RemoveFirstAsnData(byte[] array) =>
            array
                .Skip(CalcAsnFullLen(array))
                .ToArray();

        internal static bool IsRequiredContentLen(byte[] array)
        {
            int awaitedAsnLen = CalcAsnFullLen(array);
            int arrayLen = array.Length;
            return awaitedAsnLen == arrayLen;
        }

        internal static int CalcAsnLenSizeInBytes(int asnLenFirstByte) => 
            asnLenFirstByte switch
            {
                < 0x81 => 2,
                0x81 => 3,
                _ => 4
            };

        internal static int CalcAsnLenInBytes(byte[] array)=> 
            array[1] switch
            {
                < 0x81 => array[1],
                0x81 => array[2],
                _ => array.Length > 2
                    ? array[2] * 256 + array[3]
                    : -1
            };

        private static int CalcAsnFullLen(byte[] array) =>
            CalcAsnLenSizeInBytes(array[1]) + CalcAsnLenInBytes(array);
    }
}
