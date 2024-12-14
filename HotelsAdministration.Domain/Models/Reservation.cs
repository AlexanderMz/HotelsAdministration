using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using HotelsAdministration.Domain.Models.Enums;

namespace HotelsAdministration.Domain.Models;

public class Reservation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [Required]
    public string HotelId { get; set; }

    [Required]
    public string RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public List<Traveler> Guests { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    public ReservationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}