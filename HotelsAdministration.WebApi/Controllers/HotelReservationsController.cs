using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.DTOs;
using HotelsAdministration.Domain.Models.Enums;
using HotelsAdministration.Application.Interfaces;

namespace HotelsAdministration.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelReservationsController : ControllerBase
{
    private readonly IMongoCollection<Hotel> _hotelsCollection;
    private readonly IMongoCollection<Reservation> _reservationsCollection;
    private readonly IEmailService _emailService;
    private readonly ILogger<HotelReservationsController> _logger;

    /// <summary>
    /// Initializes a new instance of the HotelReservationsController
    /// </summary>
    /// <param name="database">MongoDB database instance</param>
    /// <param name="emailService">Email service for notifications</param>
    /// <param name="logger">Logger instance</param>
    public HotelReservationsController(
        IMongoDatabase database,
        IEmailService emailService,
        ILogger<HotelReservationsController> logger)
    {
        _hotelsCollection = database.GetCollection<Hotel>("hotels");
        _reservationsCollection = database.GetCollection<Reservation>("reservations");
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for available hotels based on specified criteria
    /// </summary>
    /// <param name="searchDto">Search criteria including city and number of guests</param>
    /// <returns>A list of hotels matching the search criteria</returns>
    /// <response code="200">Returns the list of matching hotels</response>
    /// <response code="500">If there was an internal error during the search</response>
    [HttpPost("search")]
    [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Hotel>>> SearchHotels(HotelSearchDto searchDto)
    {
        try
        {
            var filter = Builders<Hotel>.Filter.And(
                Builders<Hotel>.Filter.Eq(h => h.City, searchDto.City),
                Builders<Hotel>.Filter.ElemMatch(h => h.Rooms,
                    r => r.Capacity >= searchDto.GuestsCount && r.IsAvailable)
            );

            var hotels = await _hotelsCollection
                .Find(filter)
                .ToListAsync();

            return Ok(hotels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching hotels");
            return StatusCode(500, "An error occurred while searching hotels");
        }
    }

    /// <summary>
    /// Creates a new hotel reservation
    /// </summary>
    /// <param name="reservationDto">Reservation details including hotel, room, and guest information</param>
    /// <returns>The created reservation details</returns>
    /// <response code="201">Returns the newly created reservation</response>
    /// <response code="400">If the room is not available</response>
    /// <response code="404">If the hotel was not found</response>
    /// <response code="500">If there was an internal error during reservation creation</response>
    [HttpPost("reserve")]
    [ProducesResponseType(typeof(Reservation), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Reservation>> CreateReservation(CreateReservationDto reservationDto)
    {
        try
        {
            var hotel = await _hotelsCollection
                .Find(h => h.Id == reservationDto.HotelId)
                .FirstOrDefaultAsync();

            if (hotel == null)
            {
                return NotFound("Hotel not found");
            }

            var room = hotel.Rooms.FirstOrDefault(r => r.Id == reservationDto.RoomId);
            if (room == null || !room.IsAvailable)
            {
                return BadRequest("Room not available");
            }

            var reservation = new Reservation
            {
                HotelId = reservationDto.HotelId,
                RoomId = reservationDto.RoomId,
                CheckInDate = reservationDto.CheckInDate,
                CheckOutDate = reservationDto.CheckOutDate,
                Guests = reservationDto.Guests.Select(g => new Traveler
                {
                    FullName = g.FullName,
                    BirthDate = g.BirthDate,
                    Gender = g.Gender,
                    DocumentType = g.DocumentType,
                    DocumentNumber = g.DocumentNumber,
                    Email = g.Email,
                    ContactPhone = g.ContactPhone,
                    EmergencyContact = g.EmergencyContact
                }).ToList(),
                TotalPrice = CalculateTotalPrice(room.PricePerNight, reservationDto.CheckInDate, reservationDto.CheckOutDate),
                Status = ReservationStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };

            await _reservationsCollection.InsertOneAsync(reservation);
            await _emailService.SendReservationConfirmationAsync(reservation, hotel);

            return CreatedAtAction(
                nameof(GetReservationById),
                new { id = reservation.Id },
                reservation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating reservation");
            return StatusCode(500, "An error occurred while creating the reservation");
        }
    }

    /// <summary>
    /// Retrieves a specific reservation by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the reservation</param>
    /// <returns>The reservation details</returns>
    /// <response code="200">Returns the requested reservation</response>
    /// <response code="404">If the reservation was not found</response>
    /// <response code="500">If there was an internal error while retrieving the reservation</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Reservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Reservation>> GetReservationById(string id)
    {
        try
        {
            var reservation = await _reservationsCollection
                .Find(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving reservation");
            return StatusCode(500, "An error occurred while retrieving the reservation");
        }
    }

    /// <summary>
    /// Calculates the total price for a reservation
    /// </summary>
    /// <param name="pricePerNight">Price per night for the room</param>
    /// <param name="checkIn">Check-in date</param>
    /// <param name="checkOut">Check-out date</param>
    /// <returns>The total price for the stay</returns>
    private decimal CalculateTotalPrice(decimal pricePerNight, DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return pricePerNight * nights;
    }
}
