using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Presentation.Controllers;

namespace UnitTests;

public class LimiteControllerUnitTests : UnitTestsBase
{
    private readonly LimiteController _sut;

    public LimiteControllerUnitTests()
    {
        _sut = _fixture.Create<LimiteController>();
    }

    [Fact]
    public async Task Post_RetornaOk()
    {
        var response = await _sut.Post();
        Assert.IsType<OkObjectResult>(response);
    }
}