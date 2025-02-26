using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.UseCase.Certificates.Save;
using PKInfo.Utility;
using PKInfoLib.Tests.DAL;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PKInfoLib.Tests.Certificate
{
    public class SaveCertTests
    {
        private static readonly IRepository __repo = new Repository(new TestDatabase());
        private static readonly ICertWriter __certWriter = new TestCertWriter();

        [Theory]
        [MemberData(nameof(PathsAndExpectedResults))]
        public void Save_cert_from_keyContainer(string containerPath, string expected)
        {
            var sut = CreateSaveCertInteractor();

            var realized  = sut.Execute(containerPath);

            realized.Should().StartWithEquivalentOf(expected);
        }

        [Fact]
        public void Save_Certs_from_keyContainers()
        {
            var paths = PathsAndExpectedResults()
                .Select(x => (string)x[0]);
            var allExpected = PathsAndExpectedResults()
                .Select(x => (string)x[1])
                .ToList();
            var sut = CreateSaveCertsInteractor();

            var allRealized = sut.Execute(paths);
            
            for(int i=0; i < allRealized.Count; i++)
                allRealized[i].Should().StartWithEquivalentOf(allExpected[i]);
        }

        public static List<object[]> PathsAndExpectedResults() =>
        [   // format: keyContainer path, save result startWith
            [ "A:\\testKey1.000", "D:\\downloads" ],    // exist keyContainer with cert
            [ "E:\\testKey7.000", Txt.KeyContainer ],   // exist keyContainer without cert
            [ "E:\\testKey8.000", Txt.CLRException ],   // exist keyContainer with bad cert data
            [ "D:\\errorKey.000", Txt.KeyContainer ],   // bad cert format exist keyContainer
            [ "D:\\notExist.000", Txt.KeyContainer ],   // not exist keyContainer
        ];

        private static SaveCertInteractor CreateSaveCertInteractor()
            => new(__repo, __certWriter);

        private static SaveCertsInteractor CreateSaveCertsInteractor()
            => new(__repo, __certWriter);
    }
}
