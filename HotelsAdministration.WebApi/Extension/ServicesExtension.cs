using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Application.Services;
using HotelsAdministration.Infrastructure.Repositories;
using HotelsAdministration.Infrastructure.UnitOfWork;

namespace HotelsAdministration.WebApi.Extension;

public static class ServicesExtension
{
    public static void Services(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IHotelReservationRepository, HotelReservationRepository>();
        services.AddScoped<ITravelerRepository, TravelerRepository>();
    }
}
