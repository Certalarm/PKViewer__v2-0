using FluentAssertions;
using FluentAssertions.Execution;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.UseCase.Reports.ReadersErrors;
using PKInfoLib.Tests.DAL;
using System;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.Report
{
    public class ReadersErrorsTests
    {
        private static readonly DateTime __currentDate = BeginDateWithOffset(45, 305);
        private static readonly IRepository __repo = new Repository(new TestDatabase());

        [Fact]
        public void Get_content_shouldBe_1()
        {
            var sut = CreateReadersErrorsInteractor();

            var realized = sut.Execute();

            realized.Content.Should().HaveCount(1);
        }

        [Fact]
        public void Get_all_readersErrors_shouldBe_3()
        {
            using var scope = new AssertionScope(); // нужно для увеличения вложенности списков
            scope.FormattingOptions.MaxDepth = 8;               // при ошибке, когда выводятся внутренние item'ы списков (default = 5)

            var sut = CreateReadersErrorsInteractor();

            var realized = sut.Execute();

            realized.ReaderErrors.Should().HaveCount(3);
        }


        private static RootErrorInteractor CreateReadersErrorsInteractor() =>
            new(__repo, __currentDate);

    }
}
