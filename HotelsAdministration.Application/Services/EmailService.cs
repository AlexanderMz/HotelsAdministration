using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Domain.Models;

namespace HotelsAdministration.Application.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendReservationConfirmationAsync(Reservation reservation, Hotel hotel)
    {
        try
        {
            // Aquí iría la implementación del envío de correo
            // Usando SendGrid, SMTP, etc.
            _logger.LogInformation("Confirmation email sent for reservation: {ReservationId}", reservation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending confirmation email");
            throw;
        }
    }
}