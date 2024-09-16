// using FluentAssertions;
// using NetArchTest.Rules;

// namespace MinimalTranslator.Architecture.Tests;

// public class LayerTests : BaseTest
// {
//     [Test]
//     public void Domain_Should_NotHaveDependencyOnApplicationDatabaseOrApi()
//     {
//         var result = Types.InAssembly(DomainAssembly)
//             .Should()
//             .NotHaveDependencyOnAny("Application","Api", "Database")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }

//     [Test]
//     public void Database_Should_NotHaveDependencyOnApplicationOrApi()
//     {
//         var result = Types.InAssembly(DatabaseAssembly)
//             .Should()
//             .NotHaveDependencyOnAny("Application", "Api")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }

//     [Test]
//     public void Application_Should_NotHaveDependencyOnApi()
//     {
//         var result = Types.InAssembly(ApplicationAssembly)
//             .Should()
//             .NotHaveDependencyOn("Api")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }

//     [Ignore("Positive dependency checks not working for some reason")]
//     [Test]
//     public void Api_Should_HaveDependencyOnApplication()
//     {
//         var result = Types.InAssembly(ApiAssembly)
//             .Should()
//             .HaveDependencyOn("Application")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }

//     [Ignore("Positive dependency checks not working for some reason")]
//     [Test]
//     public void Application_Should_HaveDependencyOnDomain()
//     {
//         var result = Types.InAssembly(ApplicationAssembly)
//             .Should()
//             .HaveDependencyOn("Domain")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }

//     [Ignore("Positive dependency checks not working for some reason")]
//     [Test]
//     public void Database_Should_HaveDependencyOnDomain()
//     {
//         var result = Types.InAssembly(DatabaseAssembly)
//             .Should()
//             .HaveDependencyOn("Domain")
//             .GetResult();

//         result.IsSuccessful.Should().BeTrue();
//     }
// }