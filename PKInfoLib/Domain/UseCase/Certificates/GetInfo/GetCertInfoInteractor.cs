using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using static PKInfo.Domain.UseCase.Certificates.CertHelper;
using static PKInfo.Domain.UseCase.Certificates.GetInfo.CertInfoHelper;

namespace PKInfo.Domain.UseCase.GetInfo.Certificates
{
    internal sealed class GetCertInfoInteractor : BaseInteractor
    {
        #region .ctors
        public GetCertInfoInteractor(IRepository repository) : base(repository)
        {
        }
        #endregion

        internal CertInfo Execute(string containerPath)
        {
            var containerInfo = __repository.GetKeyContainerInfo(containerPath);
            var error = IsBadParam(containerInfo);
            return error.Length > 0
                ? GetCertInfoErrorOnly(error)
                : GetCertInfoByRawData(containerInfo.CertRawData);
        }

        private static CertInfo GetCertInfoByRawData(byte[] certRawData)
        {
            var error = TryGetX509Cert(certRawData, out var x509Cert);
            if (error.Length > 0)
                return GetCertInfoErrorOnly(error);
            var certInfo = GetCertInfoByX509Cert(x509Cert);
            x509Cert?.Dispose();
            return certInfo;
        }

        private static CertInfo GetCertInfoErrorOnly(string error) =>
            new()
            {
                Error = error,
            };
    }
}
