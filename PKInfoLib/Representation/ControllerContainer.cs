using PKInfo.Data.Implementation.CertWriter;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Implementation.Reporter;
using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using PKInfo.Domain.UseCase.Certificates.Save;
using PKInfo.Domain.UseCase.Delete.KeyContainers;
using PKInfo.Domain.UseCase.GetInfo.Certificates;
using PKInfo.Domain.UseCase.KeyContainers.Get;
using PKInfo.Domain.UseCase.KeyMedias.Get;
using PKInfo.Domain.UseCase.Readers.Get;
using PKInfo.Domain.UseCase.Reports.ReadersErrors;
using PKInfo.Domain.UseCase.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using PKInfo.Utility.Enum;

namespace PKInfo.Representation
{
    public static class ControllerContainer
    {
        private static readonly Repository _repo = new(new Database());
        private static DateTime _snapshotTime = DateTime.Now;

        public static CertInfo GetCertInfo(string containerPath)
        {
            var interactor = new GetCertInfoInteractor(_repo);
            return interactor.Execute(containerPath);
        }

        public static string DeleteKeyContainer(string path)
        {
            UpdateData();
            var interactor = new DeleteKeyContainerInteractor(_repo);
            var error = interactor.Execute(path);
            if (error.Length < 1)
                UpdateData(isNeedTime: false);
            return error;
        }

        public static IList<string> DeleteKeyContainers(IEnumerable<string> paths)
        {
            UpdateData();
            var interactor = new DeleteKeyContainersInteractor(_repo);
            var errors = interactor.Execute(paths);
            if (errors.Count(x => x.Length > 0) != paths.Count())
                UpdateData(isNeedTime: false);
            return errors;
        }

        public static IList<KeyContainerSnapshotInfo> GetAllKeyContainers(bool needUpdate)
        {
            if (needUpdate)
                UpdateData();
            var interactor = new GetAllInteractor(_repo, _snapshotTime);
            return interactor.Execute();
        }

        public static IList<KeyContainerSnapshotInfo> GetKeyMediaKeyContainers(string mediaPath)
        {
            var interactor = new GetKeyMediaKeyContainersInteractor(_repo, _snapshotTime);
            return interactor.Execute(mediaPath);
        }

        public static IList<KeyContainerSnapshotInfo> GetReaderKeyContainers(ReaderType readerType)
        {
            var interactor = new GetReaderKeyContainersInteractor(_repo, _snapshotTime);
            return interactor.Execute(readerType);
        }

        public static string SaveCert(string containerPath)
        {
            ICertWriter certWriter = new FileCertWriter();
            SaveCertInteractor interactor = new(_repo, certWriter);
            return interactor.Execute(containerPath);
        }

        public static IList<string> SaveCerts(IEnumerable<string> containerPaths)
        {
            ICertWriter certWriter = new FileCertWriter();
            SaveCertsInteractor interactor = new(_repo, certWriter);
            return interactor.Execute(containerPaths);
        }

        public static KeyMediaSnapshotInfo GetKeyMedia(string path)
        {
            GetKeyMediaInteractor interactor = new(_repo, _snapshotTime);
            return interactor.Execute(path);
        }

        public static IEnumerable<string> GetTypesReaderHasKeyMedia(bool needUpdate) =>
            GetReaders(needUpdate)
                .Where(x => x.KeyMediasInfo.Any())
                .Select(x => x.Type.ToString());

        public static IEnumerable<string> GetAllMediaRootPaths() =>
           GetReaders(needUpdate: false)
               .Where(x => x.KeyMediasInfo.Any())
               .SelectMany(x => GetReaderMediaRootPaths(x.Type));

        public static IEnumerable<string> GetReaderMediaRootPaths(ReaderType readerType) =>
            GetReader(readerType).KeyMediasInfo.Any()
                ? GetReader(readerType).KeyMediasInfo
                    .Select(x => x.RootPath)
                : [];

        public static IList<KeyContainerSnapshotInfo> GetKeyContainersByReaderType(string strReaderType) => 
            Enum.TryParse(strReaderType, out ReaderType readerType)
                ? GetReaderKeyContainers(readerType)
                : [];

        public static RootError GetReadersErrors()
        {
            RootErrorInteractor interactor = new(_repo, _snapshotTime);
            return interactor.Execute();
        }

        public static T GetErrorReport<T>(RootError rootError) where T : class
        {
            return BaseReporter.BuildReport<T>(rootError);
        }

        public static SelfInfo GetSelfInfo(string assemblyNameWithExt)
        {
            SelfInfoInteractor interactor = new();
            return interactor.Execute(assemblyNameWithExt);
        }

        private static ReaderSnapshotInfo GetReader(ReaderType readerType)
        {
            GetReaderInteractor interactor = new(_repo, _snapshotTime);
            return interactor.Execute(readerType);
        }

        private static IList<ReaderSnapshotInfo> GetReaders(bool needUpdate)
        {
            if (needUpdate)
                UpdateData();
            GetReadersInteractor interactor = new(_repo, _snapshotTime);
            return interactor.Execute();
        }

        private static void UpdateData(bool isNeedTime = true)
        {
            _repo.Update();
            if (isNeedTime)
                _snapshotTime = DateTime.Now;
        }
    }
}
