using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using HotelsAdministration.Application.Interfaces;
using HotelsAdministration.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;

namespace HotelsAdministration.Application.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly string _fromEmail;

    public EmailService(
        ILogger<EmailService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _smtpServer = configuration["Email:SmtpServer"];
        _smtpPort = int.Parse(configuration["Email:SmtpPort"]);
        _smtpUsername = configuration["Email:Username"];
        _smtpPassword = Environment.GetEnvironmentVariable("sendgrid");
        _fromEmail = configuration["Email:FromEmail"];
    }

    public async Task SendReservationConfirmationAsync(Reservation reservation, Hotel hotel, Traveler traveler)
    {
        try
        {
            using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_smtpUsername, _smtpPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),
                Subject = $"Confirmación de Reserva - {hotel.Name}",
                Body = GenerateEmailBody(reservation, hotel),
                IsBodyHtml = true
            };

            mailMessage.To.Add(traveler.Email);

            await smtpClient.SendMailAsync(mailMessage);

            _logger.LogInformation("Confirmation email sent for reservation: {ReservationId}", reservation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending confirmation email");
            throw;
        }
    }

    private string GenerateEmailBody(Reservation reservation, Hotel hotel)
    {
        return $@"
            <html>
                <body>
                    <h2>¡Gracias por su reserva en {hotel.Name}!</h2>
                    <p>Detalles de su reserva:</p>
                    <ul>
                        <li>Número de reserva: {reservation.Id}</li>
                        <li>Fecha de entrada: {reservation.CheckInDate:dd/MM/yyyy}</li>
                        <li>Fecha de salida: {reservation.CheckOutDate:dd/MM/yyyy}</li>
                        <li>Número de huéspedes: {reservation.Guests.Count}</li>
                    </ul>
                    <p>Para cualquier consulta, no dude en contactarnos.</p>
                    <p>¡Esperamos darle la bienvenida pronto!</p>
                </body>
            </html>";
    }
}