using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.Entity;
using PKInfo.Domain.UseCase.KeyContainers.Get;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.KeyContainer
{
    public class GetAllKeyContainersTests
    {
        private static readonly DateTime _currentLocaleDate = BeginDateWithOffset(45, 305);
        private static readonly IRepository _repo = new Repository(new TestDatabase());

        [Fact]
        public void Get_all_keyContainers_shouldBe_10()
        {
            var sut = GetAllKeyContainerInteractor();

            var result = sut.Execute();

            result.Should().HaveCount(10);
        }

        [Fact]
        public void Get_all_keyConrainersWoDeleted_shouldBe_9()
        {
            var sut = GetAllKeyContainerWoDeletedInteractor();

            var result = sut.Execute();

            result.Should().HaveCount(9);
        }

        [Fact]
        public void Get_all_keyContainers_woCert_shouldBe_2()
        {
            var sut = GetAllKeyContainerInteractor();

            var result = sut.Execute()
                .Where(x => x.IsCertPresent != true);

            result.Should().HaveCount(2);
        }

        [Fact]
        public void Get_all_keyContainers_isDeleted_shouldBe_1()
        {
            var sut = GetAllKeyContainerInteractor();

            var result = sut.Execute()
                .Where(x => x.IsDeleted == true);

            result.Should().HaveCount(1);
        }

        [Fact]
        public void Get_all_keyContainers_isEncrypted_shouldBe_2()
        {
            var sut = GetAllKeyContainerInteractor();

            var result = sut.Execute()
                .Where(x => x.IsEncrypted == true);

            result.Should().HaveCount(2);
        }

        [Fact]
        public void Get_all_keyContainers_isExportable_shouldBe_2()
        {
            var sut = GetAllKeyContainerInteractor();

            var result = sut.Execute()
                .Where(x => x.IsExportable == true);

            result.Should().HaveCount(2);
        }

        [Fact]
        public void Get_all_keyContainers_someProps_shouldBe_10()
        {
            var sut = GetAllKeyContainerInteractor();

            var realized = sut.Execute()
                .Where(x =>
                    x.Path.Length > 0);

            realized.Should().HaveCount(10);
        }

        [Theory]
        [MemberData(nameof(DataDates))]
        public void Get_all_dates_keyContainers(TimeInterval expectedRemainingTime, TimeInterval expectedElapsedTime, int expectedCount)
        {
            var sut = GetAllKeyContainerInteractor();

            var realized = sut.Execute();

            int realizedCount = sut.Execute()
                .Count(x =>
            (x.RemainingTimeUntilEndKey?.Equals(expectedRemainingTime) ?? false)
                && (x.ElapsedTimeAfterCopy?.Equals(expectedElapsedTime) ?? false));

            realizedCount.Should().Be(expectedCount);
        }

        public static List<object[]> DataDates() =>
            [ // format: RemainingTimeUntilEndKey, ElapsedTimeAfterCopy, timesCount
                [ new TimeInterval(1, 1, 10, 18, 50),  new TimeInterval(0, 1, 20, 5, 10), 4],
                [ new TimeInterval(1, 1, 13, 18, 53),  new TimeInterval(0, 1, 17, 5, 07), 1],
                [ new TimeInterval(1, 1, 12, 18, 52),  new TimeInterval(0, 1, 18, 5, 08), 1],
                [ new TimeInterval(1, 1, 11, 18, 51),  new TimeInterval(0, 1, 18, 5, 09), 1],
                [ new TimeInterval(1, 1, 10, 18, 50),  new TimeInterval(0, 1, 20, 5, 10), 4],
                [ new TimeInterval(1, 1, 09, 18, 49),  new TimeInterval(0, 1, 21, 5, 11), 1],
                [ new TimeInterval(1, 1, 10, 18, 50),  new TimeInterval(0, 1, 20, 5, 10), 4],
                [ new TimeInterval(1, 1, 10, 18, 50),  new TimeInterval(0, 1, 20, 5, 10), 4],
                [ new TimeInterval(1, 1, 14, 18, 52),  new TimeInterval(0, 1, 16, 5, 08), 1],
                [ new TimeInterval(1, 1, 13, 18, 49),  new TimeInterval(0, 1, 17, 5, 11), 1],
                [ null,  null, 0],
            ];

        private static GetAllInteractor GetAllKeyContainerInteractor() 
            => new(_repo, _currentLocaleDate);
        
        private static GetAllWoDeletedInteractor GetAllKeyContainerWoDeletedInteractor() 
            => new(_repo, _currentLocaleDate);
    }
}
