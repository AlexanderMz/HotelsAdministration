using HotelsAdministration.Application.Configuration;
using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace HotelsAdministration.Infrastructure.UnitOfWork;

public class UnitOfWork(IMongoDatabase database, IOptions<MongoDbSettings> settings) : IUnitOfWork
{
    private readonly IMongoDatabase _database = database;
    private IHotelRepository _hotelRepository;
    private IHotelReservationRepository _hotelReservations;
    private ITravelerRepository _travelers;
    private bool _disposed;

    public IHotelRepository Hotels
    {
        get { return _hotelRepository ??= new HotelRepository(_database, settings); }
    }

    public IHotelReservationRepository HotelReservations
    {
        get { return _hotelReservations ??= new HotelReservationRepository(_database, settings); }
    }

    public ITravelerRepository Travelers
    {
        get { return _travelers ??= new TravelerRepository(_database, settings); }
    }

    public async Task CompleteAsync()
    {
        await Task.FromResult(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose managed resources if needed
            }
            _disposed = true;
        }
    }
}