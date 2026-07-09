using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Customer.WebApi.Domain.Model;

[BsonIgnoreExtraElements]
public class CartItem
{
    [BsonElement("productId")]
    public required string ProductId { get; set; }

    [BsonElement("productName")]
    public required string ProductName { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("addedAt")]
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}

[BsonIgnoreExtraElements]
public class Cart
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("email")]
    public required string Email { get; set; }

    [BsonElement("items")]
    public List<CartItem> Items { get; set; } = new();

    [BsonElement("isActive")]
    public bool IsActive => Items.Count > 0;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
