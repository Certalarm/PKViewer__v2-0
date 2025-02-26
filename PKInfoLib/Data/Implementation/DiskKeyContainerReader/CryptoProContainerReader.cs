using PKInfo.Data.DTO;
using System;
using System.Globalization;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.CryptoProContainerHelper;
using static PKInfo.Data.Implementation.DiskKeyContainerReader.FileMetaInfoReader;
using static PKInfo.Utility.Txt;

namespace PKInfo.Data.Implementation.DiskKeyContainerReader
{
    internal class CryptoProContainerReader : BaseDiskKeyContainerReader
    {
        #region .ctors
        internal CryptoProContainerReader(string path, int codePage = 1251) // 1251 = Win Ru CodePage
            : base(path, codePage)
        {
        }
        #endregion

        public override KeyContainerInfoDB Read()
        {
            AddFileMetaInfoData();
            if (_kci.Error.Length > 0)
                return _kci;
            AddCPNameKeyData();
            if (_kci.Error.Length > 0)
                return _kci;
            AddCPHeaderKeyData();
            return _kci;
        }

        private void AddFileMetaInfoData()
        {
            _kci.Error = ReadMetaInfoCPNameKey(_kci.Path, out var fileMetaInfo);
            if (_kci.Error.Length > 0)
                return;
            _kci.DateOfCopyUTC = GetDateOfCopyUTC(fileMetaInfo);
            _kci.Error = ReadMetaInfoCPHeaderKey(_kci.Path, out _);
        }

        private void AddCPNameKeyData()
        {
            _kci.Error = ReadCPNameKey(_kci.Path, out var nameKeyFileContent);
            if (_kci.Error.Length < 1)
                _kci.Name = GetContainerName(nameKeyFileContent);
        }

        private void AddCPHeaderKeyData()
        {
            _kci.Error = ReadCPHeaderKey(_kci.Path, out var headerKeyFileContent);
            if (_kci.Error.Length > 0)
                return;
            _kci.PublicKey = GetPublicKey(headerKeyFileContent);
            _kci.CertRawData = GetCertRawData(headerKeyFileContent);
            _kci.IsEncrypted = IsEncryptedPrivateKey(headerKeyFileContent);
            _kci.IsExportable = IsExportablePrivateKey(headerKeyFileContent);
            _kci.DateNotAfterUTC = GetPrivateKeyNotAfterUTC(headerKeyFileContent);
        }

        private static DateTime? GetDateOfCopyUTC(FileMetaInfo fileMetaInfo) =>
           fileMetaInfo.LastWriteTimeUTC;

        private string GetContainerName(byte[] data)
        {
            var containerNameRawData = Asn1DerParser.ParseContainerName(data);
            return _codePage.GetString(containerNameRawData);
        }

        private static byte[] GetPublicKey(byte[] data) =>
            Asn1DerParser.ParsePublicKey(data);

        private static byte[] GetCertRawData(byte[] data) =>
            Asn1DerParser.ParseRawCert(data);

        private static bool IsEncryptedPrivateKey(byte[] data) =>
            Asn1DerParser.ParsePrivateKeyPassword(data).Length > 0;

        private static bool? IsExportablePrivateKey(byte[] data)
        {
            var value = Asn1DerParser.ParsePrivateKeyExportable(data);
            if (IsUnknownExportableFlag(value))
                return null;
            return value[1] == Asn1DerTag.PK_EXPORTABLE_FLAG;
        }

        private static bool IsUnknownExportableFlag(byte[] bytes) =>
            bytes.Length < 2 
            || (bytes[1] != Asn1DerTag.PK_EXPORTABLE_FLAG && bytes[1] != Asn1DerTag.PK_NON_EXPORTABLE_FLAG);

        private DateTime? GetPrivateKeyNotAfterUTC(byte[] data)
        {
            var pkNotAfterUTC = Asn1DerParser.ParsePrivateKeyNotAfterUTC(data);
            if (pkNotAfterUTC.Length < 1)
                return null;
            return TryParseNotAfter(pkNotAfterUTC);
        }

        private DateTime? TryParseNotAfter(byte[] pkNotAfterRawData)
        {
            var pkNotAfterString = _codePage.GetString(pkNotAfterRawData);
            try
            {
                return DateTime.ParseExact(pkNotAfterString, DateTimeFormatZ, CultureInfo.InvariantCulture);
            }
            catch { }
            return null;
        }
    }
}
