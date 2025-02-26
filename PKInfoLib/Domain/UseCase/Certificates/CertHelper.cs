using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using PKInfo.Utility;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static PKInfo.Utility.Txt;

namespace PKInfo.Domain.UseCase.Certificates
{
    internal static class CertHelper
    {
        internal static string SaveOneCert(string containerPath, IRepository repo, ICertWriter certWriter)
        {
            var keyContainer = repo.GetKeyContainerInfo(containerPath);
            var error = IsBadParam(keyContainer);
            if (error.Length > 0)
                return error;
            error = GetSerial(keyContainer.CertRawData, out var serial);
            return error.Length > 0
                ? error
                : certWriter.Write(keyContainer.CertRawData, serial);
        }

        internal static string IsBadParam(KeyContainerInfo containerInfo)
        {
            if (!string.IsNullOrWhiteSpace(containerInfo.Error))
                return containerInfo.Error;
            if (!containerInfo.CertRawData.Any())
                return KeyContainerCertNotFound;
            return string.Empty;
        }

        internal static string TryGetX509Cert(byte[] certRawData, out X509Certificate2 x509Cert)
        {
            x509Cert = null;
            try
            {
                x509Cert = new X509Certificate2(certRawData);
            }
            catch (Exception ex)
            {
                return ex.AsError();
            }
            return string.Empty;
        }

        private static string GetSerial(byte[] certRawData, out string serial)
        {
            var error = TryGetX509Cert(certRawData, out var x509Cert);
            serial = error.Length > 0
                ? string.Empty
                : x509Cert.GetSerialNumberString();
            x509Cert?.Dispose();
            return error;
        }
    }
}
