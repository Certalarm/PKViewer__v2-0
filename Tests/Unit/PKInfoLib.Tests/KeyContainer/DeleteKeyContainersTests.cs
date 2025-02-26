using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Domain.UseCase.Delete.KeyContainers;
using PKInfoLib.Tests.DAL;
using System.Collections.Generic;
using Xunit;
using static PKInfo.Utility.Txt;

namespace PKInfoLib.Tests.KeyContainer
{
    public class DeleteKeyContainersTests
    {
        [Theory]
        [MemberData(nameof(Exist_key_containers))]
        public void Delete_exist_key_containers(string[] containerPaths)
        {
            var sut = GetDeleteKeyContainersInteractor();
            var error = string.Empty;
            var errorAfterDelete = KeyContainerNotExist;

            var result = sut.Execute(containerPaths);
            var resultAfterDelete = sut.Execute(containerPaths);

            result.Should().AllBe(error);
            resultAfterDelete.Should().AllBe(errorAfterDelete);
        }

        [Theory]
        [MemberData(nameof(Not_Exist_key_containers))]
        public void Delete_not_exist_key_containers(string[] containerPaths)
        {
            var sut = GetDeleteKeyContainersInteractor();
            var error = KeyContainerNotExist;

            var result = sut.Execute(containerPaths);

            result.Should().AllBe(error);
        }

        public static List<object[]> Exist_key_containers() =>
            [
                [ ExistKeyContainerPaths ],
            ];

        public static List<object[]> Not_Exist_key_containers() =>
            [
                [ NotExistKeyContainerPaths ],
            ];

        private static string[] ExistKeyContainerPaths =
            [
                "A:\\testKey1.000", "A:\\testKey2.000", "E:\\testKey7.000", "E:\\testKey8.000", $"A:\\{DELETED_KEY_DIR_NAME}\\delKey01.000"
            ];

        private static string[] NotExistKeyContainerPaths =
            [
                "D:\\notExist1.000", "XX:\\testKey2.000", "D:\\testKey7.X00", "D:\\testKey8.00", $"D:\\{DELETED_KEY_DIR_NAME}\\notExist1.000"
            ];

        private static DeleteKeyContainersInteractor GetDeleteKeyContainersInteractor()
        {
            var db = new TestDatabase();
            var repo = new Repository(db);
            return new DeleteKeyContainersInteractor(repo);
        }
    }
}
