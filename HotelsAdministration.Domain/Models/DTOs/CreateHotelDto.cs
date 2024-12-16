
using System.ComponentModel.DataAnnotations;

namespace HotelsAdministration.Domain.Models.DTOs;

public class CreateHotelDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Description { get; set; }
    public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
}