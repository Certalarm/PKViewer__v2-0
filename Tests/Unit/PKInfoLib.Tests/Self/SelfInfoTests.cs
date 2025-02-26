using FluentAssertions;
using PKInfo.Domain.Entity;
using PKInfo.Domain.UseCase.Self;
using System;
using Xunit;

namespace PKInfoLib.Tests.Self
{
    public class SelfInfoTests
    {
        [Fact]
        public void Get_selfInfo_0_9_0_0()
        {
            var sut = new SelfInfoInteractor();
            var expected = new SelfInfo(name: "PkInfo", version: "0.9.0.0", releaseDate: DateTime.Now);

            var realized = sut.Execute("pkinfo.dll");

            expected.Name.Should().BeEquivalentTo(realized.Name);
            expected.Version.Should().BeEquivalentTo(realized.Version);
            expected.ReleaseDate.ToShortDateString()
                .Should().BeEquivalentTo(realized.ReleaseDate.ToShortDateString());
        }
    }
}
