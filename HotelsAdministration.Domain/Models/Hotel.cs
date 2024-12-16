using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace HotelsAdministration.Domain.Models;
public class Hotel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("city")]
    public string City { get; set; }

    [BsonElement("address")]
    public string Address { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("rooms")]    
    public List<Room> Rooms { get; set; } = new List<Room>();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}


public class UpdateHotelDto
{
    public string Name { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}
