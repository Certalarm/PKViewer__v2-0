using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.UseCase.KeyContainers.Get;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.KeyContainer
{
    public class GetKeyMediaKeyContainersTests
    {
        private static readonly DateTime __currentDate = BeginDateWithOffset(45, 305);
        private static readonly IRepository __repo = new Repository(new TestDatabase());

        [Theory]
        [MemberData(nameof(DataKeyMedia))]
        public void Get_all_keyContainers(string mediaPath, int keyCount)
        {
            var sut = CreateGetInteractor();

            var realized = sut.Execute(mediaPath);

            realized.Should().HaveCount(keyCount);
        }

        [Theory]
        [MemberData(nameof(DataKeyMediaWoDeleted))]
        public void Get_all_keyContainers_wo_deleted(string mediaPath, int keyCount)
        {
            var sut = CreateGetWoDeletedInteractor();

            var realized = sut.Execute(mediaPath);

            realized.Should().HaveCount(keyCount);
        }

        public static IList<object[]> DataKeyMedia() =>
        [   // format: mediaPath, keyContainers Count
            [ "A:\\", 4 ],  // exist floppy drive
            [ "C:\\", 0 ],  // exist system local drive
            [ "D:\\", 3 ],  // exist local drive
            [ "E:\\", 3 ],  // exist flash drive
            [ "X:\\", 0 ],  // not exist drive
        ];

        public static IList<object[]> DataKeyMediaWoDeleted() =>
        [   // format: mediaPath, keyContainers Count
            [ "A:\\", 3 ],  // exist floppy drive
            [ "C:\\", 0 ],  // exist system local drive
            [ "D:\\", 3 ],  // exist local drive
            [ "E:\\", 3 ],  // exist flash drive
            [ "X:\\", 0 ],  // not exist drive
        ];

        private static GetKeyMediaKeyContainersInteractor CreateGetInteractor()
            => new(__repo, __currentDate);

        private static GetKeyMediaKeyContainersWoDeletedInteractor CreateGetWoDeletedInteractor()
            => new(__repo, __currentDate);
    }
}
