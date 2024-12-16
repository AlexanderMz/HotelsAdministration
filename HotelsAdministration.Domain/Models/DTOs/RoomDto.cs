using HotelsAdministration.Domain.Models.Enums;

namespace HotelsAdministration.Domain.Models.DTOs;

public class RoomDto
{
    public string RoomNumber { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public List<string> Amenities { get; set; }
    public RoomStatus Status { get; set; }
}