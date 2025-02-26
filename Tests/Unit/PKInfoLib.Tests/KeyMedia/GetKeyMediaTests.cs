using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Utility.Enum;
using PKInfo.Domain.UseCase.KeyMedias.Get;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.KeyMedia
{
    public class GetKeyMediaTests
    {
        private static readonly IRepository __repo = new Repository(new TestDatabase());
        private static readonly DateTime __currentDate = BeginDateWithOffset(45, 305);

        [Theory]
        [MemberData(nameof(DataKeyMediaParams))]
        public void GetKeyMedia(string rootPath, string label, KeyMediaType kmType, int count)
        {
            var sut = CreateGetKeyMediaInteractor();

            var realized = sut.Execute(rootPath);

            realized?.Label.Should().Be(label);
            realized?.RootPath.Should().Be(rootPath);
            realized?.Size.Should().BeGreaterThan(1_023);
            realized?.Size.Should().BeLessThan(1_000_001);
            realized?.Type.Should().Be(kmType);
            realized?.KeyContainersInfo.Should().HaveCount(count);
        }

        [Theory]
        [MemberData(nameof(DataKeyMediasParams))]
        public void GetKeyMedias(KeyMediaType kmType, int kmCount, int kcCount)
        {
            var sut = CreateGetKeyMediasInteractor();

            var realized = sut.Execute(kmType);
            var realizedKeyContainers = realized
                .SelectMany(x => x.KeyContainersInfo);

            realized.Should().HaveCount(kmCount);
            realizedKeyContainers.Should().HaveCount(kcCount);
        }

        public static List<object[]> DataKeyMediaParams() =>
        [ // format: rootPath, label, keyMediaType, keyContainers count 
            [ "D:\\", "TEST LOCAL DRIVE #1", KeyMediaType.LocalDrive, 3],      // exist local drive
            [ "C:\\", "TEST SYSTEM LOCAL DRIVE", KeyMediaType.LocalDrive, 0],  // exist local system drive,
            [ "E:\\", "TEST FLASH DRIVE #1", KeyMediaType.FlashDrive, 3],      // exist flash drive
            [ "A:\\", "TEST FLOPPY DRIVE #1", KeyMediaType.FloppyDrive, 4],    // exist floppy drive
            [ "X:\\", null, KeyMediaType.NotImplemented, 0],                   // not exist drive
        ];

        public static List<object[]> DataKeyMediasParams() =>
        [   // format: keyMediaType, keyMedia Count, keyContainers Count
            [ KeyMediaType.LocalDrive, 1, 3 ],
            [ KeyMediaType.FlashDrive, 1, 3 ],
            [ KeyMediaType.FloppyDrive, 1, 4 ],
            [ KeyMediaType.NotImplemented, 0, 0],
        ];

        private static GetKeyMediaInteractor CreateGetKeyMediaInteractor() =>
            new(__repo, __currentDate);

        private static GetKeyMediasInteractor CreateGetKeyMediasInteractor() =>
            new(__repo, __currentDate);
    }
}
