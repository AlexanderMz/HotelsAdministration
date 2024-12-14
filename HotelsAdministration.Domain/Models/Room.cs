using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using HotelsAdministration.Domain.Models.Enums;

namespace HotelsAdministration.Domain.Models;

public class Room
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("roomNumber")]
    public string RoomNumber { get; set; }

    [BsonElement("type")]
    public RoomType Type { get; set; }

    [BsonElement("pricePerNight")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal PricePerNight { get; set; }

    [BsonElement("capacity")]
    public int Capacity { get; set; }

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("isAvailable")]
    public bool IsAvailable { get; set; } = true;

    [BsonElement("amenities")]
    public List<string> Amenities { get; set; } = new List<string>();

    [BsonElement("status")]
    public RoomStatus Status { get; set; } = RoomStatus.Available;
}