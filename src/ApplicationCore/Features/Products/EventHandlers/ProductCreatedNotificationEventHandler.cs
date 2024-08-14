using ApplicationCore.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationCore.Features.Products.EventHandlers
{
    public class ProductCreatedNotificationEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly ILogger<ProductCreatedNotificationEventHandler> _logger;

        public ProductCreatedNotificationEventHandler(
            ILogger<ProductCreatedNotificationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Nueva notificación: Nuevo producto {Product}", notification.Product);
            await Task.CompletedTask;
        }
    }
}
