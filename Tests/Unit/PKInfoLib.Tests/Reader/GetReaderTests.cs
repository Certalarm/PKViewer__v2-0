using FluentAssertions;
using PKInfo.Data.Implementation.DAL;
using PKInfo.Data.Interface;
using PKInfo.Domain.UseCase.Readers.Get;
using PKInfoLib.Tests.DAL;
using System;
using System.Collections.Generic;
using PKInfo.Utility.Enum;
using Xunit;
using static PKInfoLib.Tests.DAL.Generator.GeneratorHelper;

namespace PKInfoLib.Tests.Reader
{
    public class GetReaderTests
    {
        private static readonly IRepository __repo = new Repository(new TestDatabase());
        private static readonly DateTime __currentDate = BeginDateWithOffset(45, 305);

        [Theory]
        [MemberData(nameof(DataReader))]
        public void Get_reader(ReaderType rType, int keyMediaCount)
        {
            var sut = CreateGetReaderInteractor();

            var realized = sut.Execute(rType);

            realized.KeyMediasInfo.Should().HaveCount(keyMediaCount);
        }

        [Fact]
       // [MemberData(nameof(DataReaders))]
        public void Get_readers()
        {
            var sut = CreateGetReadersInteractor();

            var realized = sut.Execute();

            realized.Should().HaveCount(3);
        }

        public static IList<object[]> DataReader() =>
        [
            [ ReaderType.AllFixed, 1 ],
            [ ReaderType.AllRemovable, 2 ],
            [ ReaderType.NotImplemented, 0 ],
        ];

        //public static IList<object[]> DataReaders() =>
        //[
        //    [ ReaderType.AllFixed, 2, "" ],
        //    [ ReaderType.AllRemovable, 2, "" ],
        //    [ ReaderType.NotImplemented, 1, "" ],
        //];

        private static GetReaderInteractor CreateGetReaderInteractor() =>
            new(__repo, __currentDate);

        private static GetReadersInteractor CreateGetReadersInteractor() =>
            new(__repo, __currentDate);
    }
}
