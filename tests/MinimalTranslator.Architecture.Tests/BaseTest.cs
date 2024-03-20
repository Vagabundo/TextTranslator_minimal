using MinimalTranslator.Api.ApiData;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Database.Repositories;
using MinimalTranslator.Domain;
using MinimalTranslator.SharedKernel;
using System.Reflection;

namespace MinimalTranslator.Architecture.Tests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Entity).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(TranslationService).Assembly;
    protected static readonly Assembly ApiAssembly = typeof(TranslationRequest).Assembly;
    protected static readonly Assembly DatabaseAssembly = typeof(TranslationRepository).Assembly;
    protected static readonly Assembly SharedKernelAssembly = typeof(Result).Assembly;
}