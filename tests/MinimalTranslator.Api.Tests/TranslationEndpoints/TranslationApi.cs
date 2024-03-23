using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MinimalTranslator.Api.Tests;

public class TranslationApiTests
{
    private readonly WebApplicationFactory<Program> _webApp = new WebApplicationFactory<Program>();
    private HttpClient _api;

    [SetUp]
    public void Setup()
    {
        _api = _webApp.CreateClient();
    }

    [Test]
    public async Task PostText_WhenTextIsValid_ReturnsOK()
    {
        var addResponse = await _api.PostAsJsonAsync("/api/translation", new {
            text = "Hello world!"
        });

        addResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}