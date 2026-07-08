using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Customer.WebApi.Domain.Model;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("email")]
    public string? Email { get; set; }

    [BsonElement("name")]
    public required string Name { get; set; }

    [BsonElement("phone")]
    public required string Phone { get; set; }

    [BsonElement("externalAuthId")]
    public string? ExternalAuthId { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
