using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelsAdministration.Domain.Models.DTOs;

public class CreateReservationDto
{
    [Required]
    public string HotelId { get; set; }

    [Required]
    public string RoomId { get; set; }

    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public List<CreateTravelerDto> Guests { get; set; }
}