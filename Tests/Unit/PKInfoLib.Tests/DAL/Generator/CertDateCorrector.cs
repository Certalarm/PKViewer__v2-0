using System;
using System.Linq;
using System.Text;

namespace PKInfoLib.Tests.DAL.Generator
{
    internal static class CertDateCorrector
    {
        private static readonly byte[] __CommonPrefix = [0x30, 0x1e];
        private static readonly byte[] __DatePrefix = [0x17, 0x0d];
        private static readonly byte[] __DatesSignature = [.. __CommonPrefix, .. __DatePrefix];
        private const int __DatesLen = 32;
        private const int __Z = 0x5a;
        private const int __BIT_STRING_TAG = 0x17;

        internal static byte[] ChangeDatesInCert(byte[] certData, DateTime startDate)
        {
            var indexDates = GetIndexDatesSignature(certData);
            if (indexDates < 0)
                return certData;
            var newDatesBytes = BuildNewDates(startDate, startDate.AddMonths(15));
            return [.. certData.Take(indexDates), .. newDatesBytes, .. certData.Skip(indexDates + __DatesLen)];
        }

        private static byte[] BuildNewDates(DateTime startDate, DateTime endDate)
        {
            var startDateBytes = ToBytes(DateToZFormat(startDate.AddDays(1).AddMinutes(3)));
            var endDateBytes = ToBytes(DateToZFormat(endDate.AddDays(1).AddMinutes(3)));
            return BuildDatesBytes(startDateBytes, endDateBytes);
        }

        private static byte[] ToBytes(string value) =>
            Encoding.UTF8.GetBytes(value);

        private static string DateToZFormat(DateTime dateTime) =>
           dateTime.ToString("yyMMddHHmmss") + "Z";

        private static byte[] BuildDatesBytes(byte[] startBytes, byte[] endBytes) =>
            [.. __CommonPrefix, .. __DatePrefix, .. startBytes, .. __DatePrefix, .. endBytes];

        private static bool IsCorrectDates(byte[] datesArray)
        {
            if (GetIndexSubArray(datesArray, __DatePrefix, 14) < 0)
                return false;
            if (datesArray[16] != __Z && datesArray.Last() != __Z)
                return false;
            if (datesArray[2] != __BIT_STRING_TAG && datesArray[17] != __BIT_STRING_TAG)
                return false;
            return true;
        }

        private static byte[] GetDates(byte[] array, int startIndex) =>
            GetSubArray(array, startIndex, __DatesLen);

        private static byte[] GetSubArray(byte[] array, int startIndex, int length) =>
            array
                .Skip(startIndex)
                .Take(length)
                .ToArray();

        private static int GetIndexDatesSignature(byte[] array, int startIndex = 0)
        {
            var loopLen = array.Length - __DatesSignature.Length + 1;
            int patternIndex;
            for (var dataIndex = startIndex; dataIndex < loopLen; dataIndex++)
            {
                if (array[dataIndex] != __DatesSignature[0])
                    continue;
                for (patternIndex = __DatesSignature.Length - 1; patternIndex >= 1; patternIndex--)
                    if (array[dataIndex + patternIndex] != __DatesSignature[patternIndex])
                        break;
                if (patternIndex == 0 && IsCorrectDates(GetDates(array, dataIndex)))
                    return dataIndex;
            }
            return -1;
        }

        private static int GetIndexSubArray(byte[] array, byte[] subArray, int startIndex = 0)
        {
            var loopLen = array.Length - subArray.Length + 1;
            int patternIndex;
            for (var dataIndex = startIndex; dataIndex < loopLen; dataIndex++)
            {
                if (array[dataIndex] != subArray[0])
                    continue;
                for (patternIndex = subArray.Length - 1; patternIndex >= 1; patternIndex--)
                    if (array[dataIndex + patternIndex] != subArray[patternIndex])
                        break;
                if (patternIndex == 0)
                    return dataIndex;
            }
            return -1;
        }
    }
}
