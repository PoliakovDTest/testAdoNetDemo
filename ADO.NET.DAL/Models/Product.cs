namespace ADO.NET.DAL.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Quantity { get; set; }
    public decimal Price { get; set; }
    public bool IsPurchased { get; set; }
    public string UserName { get; set; }
    // тествовый комит 
}