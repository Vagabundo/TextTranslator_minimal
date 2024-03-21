using FluentAssertions;
using MinimalTranslator.Domain;
using NetArchTest.Rules;

namespace MinimalTranslator.Architecture.Tests;

public class DomainTests : BaseTest
{
    [Test]
    public void DomainEntities_Should_BeSealed()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Ignore("NetArchTest does not support records yet")]
    [Test]
    public void DomainEntities_Should_BeRecords()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .Should()
            .NotBeClasses()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}