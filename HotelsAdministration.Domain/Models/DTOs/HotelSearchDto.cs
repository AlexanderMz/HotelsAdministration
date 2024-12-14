using System;
using System.ComponentModel.DataAnnotations;

namespace HotelsAdministration.Domain.Models.DTOs;

public class HotelSearchDto
{
    [Required]
    public DateTime CheckInDate { get; set; }

    [Required]
    public DateTime CheckOutDate { get; set; }

    [Required]
    public int GuestsCount { get; set; }

    [Required]
    public string City { get; set; }
}