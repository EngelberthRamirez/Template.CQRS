namespace ApplicationCore.Domain;
public class Product
{
    public int Id { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public decimal Precio { get; set; }
    public int Stock { get; set; }
}
