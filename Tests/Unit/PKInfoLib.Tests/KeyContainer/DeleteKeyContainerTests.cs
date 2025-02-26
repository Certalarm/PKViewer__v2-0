using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Domain.UseCase.Delete.KeyContainers;
using PKInfoLib.Tests.DAL;
using Xunit;
using static PKInfo.Utility.Txt;

namespace PKInfoLib.Tests.KeyContainer
{
    public class DeleteKeyContainerTests
    {
        [Fact]
        public void Delete_exist_key_container()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = @"A:\testKey1.000";

            var result = sut.Execute(containerPath);
            var resultAfterDelete = sut.Execute(containerPath);

            result.Should().BeEmpty();
            resultAfterDelete.Should().Be(KeyContainerNotExist);
        }

        [Fact]
        public void Delete_key_container_with_empty_path()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = string.Empty;

            var result = sut.Execute(containerPath);

            result.Should().Be(KeyContainerEmptyPath);
        }

        [Fact]
        public void Delete_exist_key_container_from_deleted_store()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = $"A:\\{DELETED_KEY_DIR_NAME}\\delKey01.000";

            var result = sut.Execute(containerPath);
            var resultAfterDelete = sut.Execute(containerPath);

            result.Should().BeEmpty();
            resultAfterDelete.Should().Be(KeyContainerNotExist);
        }

        [Fact]
        public void Delete_not_exist_key_container()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = @"D:\notExist.000";

            var result = sut.Execute(containerPath);

            result.Should().Be(KeyContainerNotExist);
        }

        [Fact]
        public void Delete_not_exist_key_container_from_not_exist_reader()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = @"XX:\notExist.000";

            var result = sut.Execute(containerPath);

            result.Should().Be(KeyContainerNotExist);
        }

        [Fact]
        public void Delete_not_exist_key_container_from_deleted_store()
        {
            var sut = GetDeleteKeyContainerInteractor();
            var containerPath = $"D:\\{DELETED_KEY_DIR_NAME}\\notExist.000";

            var result = sut.Execute(containerPath);

            result.Should().Be(KeyContainerNotExist);
        }


        private static DeleteKeyContainerInteractor GetDeleteKeyContainerInteractor()
        {
            var db = new TestDatabase();
            var repo = new Repository(db);
            return new DeleteKeyContainerInteractor(repo);
        }
    }
}
