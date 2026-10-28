using Alsiso.Application.Quotes;
using Alsiso.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Alsiso.Infrastructure.Quotes;

public sealed class SmtpQuoteRequestNotifier(
    IConfiguration configuration) : IQuoteRequestNotifier
{
    public async Task NotifyNewQuoteAsync(
        QuoteRequest quoteRequest,
        CancellationToken cancellationToken)
    {
        var host = GetRequiredSetting("Email:Smtp:Host");

        var portValue = GetRequiredSetting("Email:Smtp:Port");

        if (!int.TryParse(portValue, out var port))
        {
            throw new InvalidOperationException(
                "La configuración Email:Smtp:Port no es un número válido.");
        }

        var username = GetRequiredSetting("Email:Smtp:Username");
        var password = GetRequiredSetting("Email:Smtp:Password");
        var from = GetRequiredSetting("Email:Smtp:From");
        var to = GetRequiredSetting("Email:Smtp:To");

        var message = new MimeMessage();

        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = $"Nueva cotización: {quoteRequest.ProductName}";

        message.Body = new TextPart("plain")
        {
            Text =
                $"""
                Se recibió una nueva solicitud de cotización.

                Producto: {quoteRequest.ProductName}
                Nombre: {quoteRequest.CustomerName}
                Correo: {quoteRequest.Email}
                Teléfono: {quoteRequest.Phone}
                Cantidad: {quoteRequest.Quantity}
                Mensaje: {quoteRequest.Message}

                Fecha: {quoteRequest.CreatedAt:O}
                """
        };

        using var smtpClient = new SmtpClient();

        //smtpClient.CheckCertificateRevocation = false; // Solo para desarrollo local

        await smtpClient.ConnectAsync(
            host,
            port,
            SecureSocketOptions.StartTls,
            cancellationToken);

        await smtpClient.AuthenticateAsync(
            username,
            password,
            cancellationToken);

        await smtpClient.SendAsync(
            message,
            cancellationToken);

        await smtpClient.DisconnectAsync(
            quit: true,
            cancellationToken);
    }

    private string GetRequiredSetting(string key)
    {
        return configuration[key] is { Length: > 0 } value
            ? value
            : throw new InvalidOperationException(
                $"Falta configurar el valor '{key}'.");
    }
}