using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Utility.Enum;
using PKInfo.Domain.UseCase.GetInfo.Certificates;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using Xunit;
using static PKInfoLib.Tests.Certificate.MemberDataStore;

namespace PKInfoLib.Tests.Certificate
{
    public class GetCertInfoTests
    {
        public static List<object[]> DataWithErrors => DataWithErrors();
        public static List<object[]> DataForCheckTypes => DataForCheckTypes();
        public static List<object[]> DataForCheckCertSomeV1Fields => DataForCheckCertSomeV1Fields();
        public static List<object[]> DataForCheckSubjectFields => DataForCheckSubjectFields();
        public static List<object[]> DataForCheckIssuerFields => DataForCheckIssuerFields();

        [Theory]
        [MemberData(nameof(DataWithErrors))]
        public void Check_errors_with_bad_data(string containerPath, string error)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Error.Should().StartWith(error);
        }

        [Theory]
        [MemberData(nameof(DataWithErrors))]
        public void Check_allFields_with_bad_data(string containerPath, string _)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Error.Should().NotBeEmpty();
            result.Serial.Should().BeNull();
            result.NotBeforeUTC.Should().BeNull();
            result.NotAfterUTC.Should().BeNull();
            result.Type.Should().Be(CertType.Unknown);
            result.OwnerType.Should().Be(CertOwnerType.Unknown);
            result.Organization.Should().BeNull();
            result.OrganizationUnit.Should().BeNull();
            result.Owner.Should().BeNull();
            result.OwnerTitle.Should().BeNull();
            result.OwnerEmail.Should().BeNull();
            result.Issuer.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(DataForCheckTypes))]
        public void Check_certType_and_certOwnerType(string containerPath, CertType cType, CertOwnerType coType)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Type.Should().Be(cType);
            result.OwnerType.Should().Be(coType);
        }

        [Theory]
        [MemberData(nameof(DataForCheckCertSomeV1Fields))]
        public void Check_serial_and_both_dates(string containerPath, string serial, DateTime? notBefore, DateTime? notAfter)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Serial.Should().Be(serial);
            result.NotBeforeUTC.Should().Be(notBefore);
            result.NotAfterUTC.Should().Be(notAfter);
        }

        [Theory]
        [MemberData(nameof(DataForCheckSubjectFields))]
        public void Check_subject_fields(string containerPath, string O, string OU, string owner, string T, string E)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Organization.Should().Be(O);
            result.OrganizationUnit.Should().Be(OU);
            result.Owner.Should().Be(owner);
            result.OwnerTitle.Should().Be(T);
            result.OwnerEmail.Should().Be(E);
        }

        [Theory]
        [MemberData(nameof(DataForCheckIssuerFields))]
        public void Check_issuer_fields(string containerPath, string issuer)
        {
            var sut = CreateGetCertInfoInteractor();

            var result = sut.Execute(containerPath);

            result.Issuer.Should().Be(issuer);
        }

        private static GetCertInfoInteractor CreateGetCertInfoInteractor()
        {
            var db = new TestDatabase();
            var repo = new Repository(db);
            return new GetCertInfoInteractor(repo);
        }
    }
}
