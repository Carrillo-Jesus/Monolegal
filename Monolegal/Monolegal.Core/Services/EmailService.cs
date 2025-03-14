using System.Net;
using System.Net.Mail;
using Monolegal.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Monolegal.Core.Services
{
    public class EmailService: IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailToClient(string recipient, string subject, string client, string code, string status)
        {
            try
            {
                // Configuración de los parámetros del correo
                var senderEmail = _configuration["SmtpSettings:SmtpUsername"];
                var senderPassword = _configuration["SmtpSettings:SmtpPassword"];
                var smtpServer = _configuration["SmtpSettings:SmtpServer"];
                var smtpPort = 587;

                var message = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = GenerateEmailBody(client, code, status),
                    IsBodyHtml = true 
                };
                message.To.Add(new MailAddress(recipient));

                // Configura el cliente SMTP para Gmail
                using (var clientSmtp = new SmtpClient(smtpServer, smtpPort))
                {
                    clientSmtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    clientSmtp.EnableSsl = true;  // Usa SSL para la conexión

                    // Enviar el correo
                    await clientSmtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes logear el error si es necesario)
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
        }

        private string GenerateEmailBody(string client, string code, string status)
        {
            // Generar el cuerpo del correo en formato HTML
            return $@"
            <html>
            <body>
                <h3>Hola {client},</h3>
                <p>Tu estado es: <strong>{status}</strong></p>
                <p>Código: {code}</p>
                <p>Gracias por confiar en nosotros.</p>
            </body>
            </html>";
        }
    }
}