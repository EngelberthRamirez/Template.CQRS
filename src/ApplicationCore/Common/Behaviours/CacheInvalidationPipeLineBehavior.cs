using MediatR;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Caching;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Messaging;

namespace PJENL.API.CleanArchitecture.ApplicationCore.Common.Behaviours;

public class CacheInvalidationPipeLineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : ICacheInvalidationCommand
{
    private readonly ICacheService _cacheService;

    public CacheInvalidationPipeLineBehavior(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        if (request is ICacheInvalidationCommand cacheInvalidationCommand)
        {
            cacheInvalidationCommand.CacheKeys.ToList().ForEach(cacheKey => _cacheService.Remove(cacheKey));
        }

        return response;
    }
}
