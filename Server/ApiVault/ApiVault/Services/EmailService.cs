using System.Net.Mail;
using System.Net;
using ApiVault.Interfaces;

namespace ApiVault.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendWelcomeEmailAsync(string toEmail)
        {
            try
            {
                // Configuración SMTP desde appsettings.json
                var smtpHost = _configuration["EmailSettings:SmtpHost"];
                var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var senderPassword = _configuration["EmailSettings:SenderPassword"];
                var senderName = _configuration["EmailSettings:SenderName"];

                using var client = new SmtpClient(smtpHost, smtpPort);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail!, senderName),
                    Subject = "¡Bienvenido a nuestra Comunidad!",
                    Body = GetEmailTemplate(toEmail),
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // Log del error (puedes usar ILogger aquí)
                Console.WriteLine($"Error enviando email: {ex.Message}");
                return false;
            }
        }

        private string GetEmailTemplate(string email)
        {
            return $@"
    <!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1'>
    <title>Bienvenido a la Comunidad</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px;
            text-align: center;
            border-radius: 10px 10px 0 0;
        }}
        .content {{
            background: #f9f9f9;
            padding: 30px;
            border-radius: 0 0 10px 10px;
        }}
        .button {{
            display: inline-block;
            padding: 12px 30px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-decoration: none;
            border-radius: 25px;
            margin: 20px 0;
        }}
        .footer {{
            text-align: center;
            margin-top: 30px;
            font-size: 12px;
            color: #666;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>¡Bienvenido a la Comunidad!</h1>
    </div>
    <div class='content'>
        <h2>Hola,</h2>
        <p>¡Gracias por suscribirte a nuestra comunidad! Estamos emocionados de tenerte con nosotros.</p>
        
        <p>Como miembro de nuestra comunidad, recibirás:</p>
        <ul>
            <li>📧 Las últimas noticias y actualizaciones</li>
            <li>🎁 Ofertas exclusivas solo para suscriptores</li>
            <li>🚀 Acceso anticipado a nuevos lanzamientos</li>
            <li>💡 Contenido exclusivo y tips útiles</li>
        </ul>
        
        <p>Mantente atento a tu bandeja de entrada para recibir contenido increíble.</p>
        
        <center>
            <a href='#' class='button'>Visitar Comunidad</a>
        </center>
        
        <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
        
        <p>¡Gracias por unirte!</p>
        <p><strong>El Equipo de la Comunidad</strong></p>
    </div>
    <div class='footer'>
        <p>Este email fue enviado a {email}</p>
        <p>Si no te suscribiste a esta lista, puedes ignorar este email.</p>
    </div>
</body>
</html>";
        }
    }
}