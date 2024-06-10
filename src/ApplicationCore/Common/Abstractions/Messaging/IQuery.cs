using MediatR;

namespace PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
