using PKInfo.Domain.Entity;
using PKInfo.Representation;
using PKInfo.Utility.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WpfMvvm.Models
{
    internal static class DataModelsLoader
    {
        internal static SelfInfo GetSelfInfo(string assemblyNameWithExt) => 
            ControllerContainer.GetSelfInfo(assemblyNameWithExt);

        internal static async Task<IList<KeyContainerSnapshotInfo>> GetAllKeyContainersAsync(bool needUpdate) => 
            await Task.Run(() => ControllerContainer.GetAllKeyContainers(needUpdate))
                .ConfigureAwait(false);

        internal static async Task<IEnumerable<string>> GetTypesReaderWithKeyMediaAsync(bool needUpdate) =>
            await Task.Run(() => ControllerContainer.GetTypesReaderHasKeyMedia(needUpdate))
                .ConfigureAwait(false);

        internal static async Task<IEnumerable<string>> GetReaderMediaRootPathsAsync(ReaderType readerType) =>
            await Task.Run(() => ControllerContainer.GetReaderMediaRootPaths(readerType))
                .ConfigureAwait(false);

        internal static async Task<IEnumerable<string>> GetAllMediaRootPathsAsync() =>
            await Task.Run(() => ControllerContainer.GetAllMediaRootPaths())
                .ConfigureAwait(false);

        internal static async Task<IList<KeyContainerSnapshotInfo>> GetKeyContainersByMediaPathAsync(string rootPath) =>
            await Task.Run(() => ControllerContainer.GetKeyMediaKeyContainers(rootPath))
                .ConfigureAwait(false);

        internal static async Task<IList<KeyContainerSnapshotInfo>> GetKeyContainersByReaderTypeAsync(string strReaderType) =>
            await Task.Run(() => ControllerContainer.GetKeyContainersByReaderType(strReaderType))
                .ConfigureAwait(false);

        internal static CertInfo GetCertInfo(string containerPath) => 
            ControllerContainer.GetCertInfo(containerPath);

        internal static string SaveCert(string containerPath) => 
            ControllerContainer.SaveCert(containerPath);

        internal static IList<string> SaveCerts(IEnumerable<string> containerPaths) => 
            ControllerContainer.SaveCerts(containerPaths);

        internal static string DeleteContainer(string containerPath) => 
            ControllerContainer.DeleteKeyContainer(containerPath);

        internal static IList<string> DeleteContainers(IEnumerable<string> containerPaths) => 
            ControllerContainer.DeleteKeyContainers(containerPaths);

        internal static KeyMediaSnapshotInfo GetKeyMediaInfo(string path) =>
            ControllerContainer.GetKeyMedia(path);

        internal static RootError GetErrors() =>
            ControllerContainer.GetReadersErrors();

        internal static string GetPlainTextErrorReport(RootError rootError) =>
            ControllerContainer.GetErrorReport<string>(rootError);
    }
}
