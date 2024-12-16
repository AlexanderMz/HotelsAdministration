using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsAdministration.Domain.Models.DTOs;

public class CreateReservationDto
{
    [Required]
    public string HotelId { get; set; }

    [Required]
    public string RoomNumber { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public List<CreateTravelerDto> Guests { get; set; }

    [Required]
    public EmergencyContact EmergencyContact { get; set; }
}