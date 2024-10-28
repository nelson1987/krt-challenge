using AutoFixture;
using AutoFixture.AutoMoq;

namespace UnitTests;

public class UnitTestsBase
{
    protected IFixture _fixture = new Fixture()
        .Customize(new AutoMoqCustomization
        {
            ConfigureMembers = true
        });
}
