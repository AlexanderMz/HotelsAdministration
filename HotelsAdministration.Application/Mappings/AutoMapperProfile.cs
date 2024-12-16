// HotelsAdministration.Application/Mappings/AutoMapperProfile.cs
using AutoMapper;
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.DTOs;

namespace HotelsAdministration.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Hotel Mappings
        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(_ => _.Rooms));

        CreateMap<UpdateHotelDto, Hotel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


        // Room Mappings
        CreateMap<RoomDto, Room>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));


        // Traveler Mappings
        CreateMap<CreateTravelerDto, Traveler>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Reservation Mappings
        CreateMap<CreateReservationDto, Reservation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pending"));

    }
}