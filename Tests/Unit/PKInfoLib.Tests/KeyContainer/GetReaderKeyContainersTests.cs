using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.UseCase.KeyContainers.Get;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using PKInfo.Utility.Enum;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;


namespace PKInfoLib.Tests.KeyContainer
{
    public class GetReaderKeyContainersTests
    {
        private static readonly DateTime __currentDate = BeginDateWithOffset(45, 305);
        private static readonly IRepository __repo = new Repository(new TestDatabase());

        [Theory]
        [MemberData(nameof(DataReader))]
        public void Get_containers(ReaderType rType, int keyCount)
        {
            var sut = CreateGetInteractor();

            var realized = sut.Execute(rType);

            realized.Should().HaveCount(keyCount);
        }

        [Theory]
        [MemberData(nameof(DataReaderWoDeleted))]
        public void Get_containers_wo_deleted(ReaderType rType, int keyCount)
        {
            var sut = CreateGetWoDeletedInteractor();

            var realized = sut.Execute(rType);

            realized.Should().HaveCount(keyCount);
        }

        public static IList<object[]> DataReader() =>
        [   // format: readerType, keyContainers Count
            [ ReaderType.AllRemovable, 7 ],     // exist floppy and flash drives
            [ ReaderType.AllFixed, 3 ],         // exist system local drive and local drive D
            [ ReaderType.NotImplemented, 0 ],   // exist notImplemented drive
        ];

        public static IList<object[]> DataReaderWoDeleted() =>
        [   // format: readerType, keyContainers Count
            [ ReaderType.AllRemovable, 6 ],     // exist floppy and flash drives
            [ ReaderType.AllFixed, 3 ],         // exist system local drive and local drive D
            [ ReaderType.NotImplemented, 0 ],   // exist notImplemented drive
        ];

        private static GetReaderKeyContainersInteractor CreateGetInteractor()
            => new(__repo, __currentDate);

        private static GetReaderKeyContainersWoDeletedInteractor CreateGetWoDeletedInteractor()
            => new(__repo, __currentDate);
    }
}
