using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelsAdministration.WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelsController> _logger;

    public HotelsController(IUnitOfWork unitOfWork, ILogger<HotelsController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all hotels in the system
    /// </summary>
    /// <returns>A collection of all hotels</returns>
    /// <response code="200">Returns the list of hotels</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Hotel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
    {
        try
        {
            var hotels = await _unitOfWork.Hotels.GetAllAsync();
            return Ok(hotels);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotels");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Retrieves a specific hotel by its identifier
    /// </summary>
    /// <param name="id">The unique identifier of the hotel</param>
    /// <returns>The requested hotel</returns>
    /// <response code="200">Returns the requested hotel</response>
    /// <response code="404">If the hotel was not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Hotel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Hotel>> GetHotel(string id)
    {
        var hotel = await _unitOfWork.Hotels.GetByIdAsync(id);
        if (hotel == null)
            return NotFound();

        return Ok(hotel);
    }

    /// <summary>
    /// Creates a new hotel in the system
    /// </summary>
    /// <param name="hotel">The hotel information to create</param>
    /// <returns>The created hotel</returns>
    /// <response code="201">Returns the newly created hotel</response>
    /// <response code="400">If the hotel data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Hotel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
    {
        try
        {
            await _unitOfWork.Hotels.CreateAsync(hotel);
            await _unitOfWork.CompleteAsync();
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating hotel");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing hotel's information
    /// </summary>
    /// <param name="id">The unique identifier of the hotel to update</param>
    /// <param name="hotel">The updated hotel information</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the hotel was successfully updated</response>
    /// <response code="400">If the hotel data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateHotel(string id, Hotel hotel)
    {
        try
        {
            await _unitOfWork.Hotels.UpdateAsync(id, hotel);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating hotel");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Toggles the active status of a hotel
    /// </summary>
    /// <param name="id">The unique identifier of the hotel</param>
    /// <param name="status">The new status to set</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the status was successfully updated</response>
    /// <response code="404">If the hotel was not found</response>
    [HttpPatch("{id}/toggle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleHotelStatus(string id, [FromBody] bool status)
    {
        var result = await _unitOfWork.Hotels.ToggleHotelStatusAsync(id, status);
        if (!result)
            return NotFound();

        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    /// <summary>
    /// Toggles the active status of a specific room in a hotel
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel</param>
    /// <param name="roomNumber">The room number to toggle</param>
    /// <param name="status">The new status to set</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the room status was successfully updated</response>
    /// <response code="404">If the hotel or room was not found</response>
    [HttpPatch("{hotelId}/rooms/{roomNumber}/toggle")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleRoomStatus(
        string hotelId,
        string roomNumber,
        [FromBody] bool status)
    {
        var result = await _unitOfWork.Hotels.ToggleRoomStatusAsync(hotelId, roomNumber, status);
        if (!result)
            return NotFound();

        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    /// <summary>
    /// Updates the information of a specific room in a hotel
    /// </summary>
    /// <param name="hotelId">The unique identifier of the hotel</param>
    /// <param name="room">The updated room information</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">If the room was successfully updated</response>
    /// <response code="404">If the hotel or room was not found</response>
    [HttpPut("{hotelId}/rooms")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRoom(string hotelId, Room room)
    {
        var result = await _unitOfWork.Hotels.UpdateRoomAsync(hotelId, room);
        if (!result)
            return NotFound();

        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
