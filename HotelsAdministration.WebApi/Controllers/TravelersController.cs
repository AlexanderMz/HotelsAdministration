using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using HotelsAdministration.Domain.Models;
using HotelsAdministration.Domain.Models.DTOs;
using HotelsAdministration.Application.Interfaces;

namespace HotelsAdministration.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TravelersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TravelersController> _logger;

    public TravelersController(
        IUnitOfWork unitOfWork,
        ILogger<TravelersController> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new traveler in the system
    /// </summary>
    /// <param name="travelerDto">The traveler information to create</param>
    /// <returns>The newly created traveler</returns>
    /// <response code="201">Returns the newly created traveler</response>
    /// <response code="400">If the traveler data is invalid</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(Traveler), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Traveler>> CreateTraveler(CreateTravelerDto travelerDto)
    {
        try
        {
            var traveler = new Traveler
            {
                FullName = travelerDto.FullName,
                BirthDate = travelerDto.BirthDate,
                Gender = travelerDto.Gender,
                DocumentType = travelerDto.DocumentType,
                DocumentNumber = travelerDto.DocumentNumber,
                Email = travelerDto.Email,
                ContactPhone = travelerDto.ContactPhone,
                EmergencyContact = travelerDto.EmergencyContact
            };

            await _unitOfWork.Travelers.CreateTravelerAsync(traveler);
            _logger.LogInformation("Traveler created successfully: {TravelerId}", traveler.Id);

            return CreatedAtAction(
                nameof(GetTravelerById),
                new { id = traveler.Id },
                traveler);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating traveler");
            return StatusCode(500, "An error occurred while creating the traveler");
        }
    }

    /// <summary>
    /// Retrieves a specific traveler by their ID
    /// </summary>
    /// <param name="id">The unique identifier of the traveler</param>
    /// <returns>The requested traveler information</returns>
    /// <response code="200">Returns the requested traveler</response>
    /// <response code="404">If the traveler was not found</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Traveler), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Traveler>> GetTravelerById(string id)
    {
        try
        {
            var traveler = await _unitOfWork.Travelers.GetTravelerByIdAsync(id);                

            if (traveler == null)
            {
                return NotFound();
            }

            return traveler;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving traveler");
            return StatusCode(500, "An error occurred while retrieving the traveler");
        }
    }
}