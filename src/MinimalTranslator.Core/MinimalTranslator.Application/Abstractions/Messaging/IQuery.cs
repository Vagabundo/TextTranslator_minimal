using MediatR;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}