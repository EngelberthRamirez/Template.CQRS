using ApplicationCore.Domain.Events;

namespace ApplicationCore.Domain.Entities;
public class Product : IHasDomainEvent
{
    private readonly List<DomainEvent> _domainEvents = [];

    public int Id { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public List<DomainEvent> DomainEvents => _domainEvents;

    private Product() { }
    public Product(int id, string nombre, decimal precio, int stock)
    {
        if (precio <= 0) throw new ArgumentException("Precio cannot be zero or negative.");
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre cannot be empty.");
        Id = id;
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
    }

    public static Product Create(int id, string nombre, decimal precio, int stock)
    {
        var product = new Product(id, nombre, precio, stock);
        product.AddDomainEvent(new ProductCreatedEvent(product));
        return product;
    }

    private void AddDomainEvent(DomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }
}