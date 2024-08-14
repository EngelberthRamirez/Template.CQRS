using ApplicationCore.Domain.Entities;

namespace ApplicationCore.Domain.Events
{
    public class ProductCreatedEvent : DomainEvent
    {
        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }
}
