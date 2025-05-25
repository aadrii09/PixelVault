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
    <title>¡Bienvenido a la Comunidad Gamer!</title>
    <link href='https://fonts.googleapis.com/css2?family=Press+Start+2P&display=swap' rel='stylesheet'>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background: #181a27;
            color: #fff;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 650px;
            margin: 30px auto;
            background: #23263a;
            border-radius: 18px;
            overflow: hidden;
            box-shadow: 0 0 24px #000a;
        }}
        .header {{
            background: linear-gradient(90deg, #7f5cff 0%, #00e6f6 100%);
            color: #fff;
            text-align: center;
            padding: 40px 20px 30px 20px;
        }}
        .header h1 {{
            font-family: 'Press Start 2P', 'Consolas', monospace;
            font-size: 2.1rem;
            margin: 0;
            letter-spacing: 2px;
            text-shadow: 0 2px 8px #0008;
            color: #fff;
        }}
        .header .gamer-icon {{
            font-size: 3rem;
            margin-bottom: 10px;
            display: block;
        }}
        .content {{
            padding: 36px 32px 32px 32px;
            background: #23263a;
            color: #fff;
        }}
        .content h2 {{
            font-family: 'Press Start 2P', 'Consolas', monospace;
            color: #00e6f6;
            font-size: 1.1rem;
            margin-top: 0;
        }}
        .features {{
            margin: 28px 0 24px 0;
            padding: 0;
            list-style: none;
            color: #fff;
        }}
        .features li {{
            margin-bottom: 14px;
            font-size: 1.08rem;
            display: flex;
            align-items: center;
            color: #fff;
        }}
        .features .emoji {{
            font-size: 1.4rem;
            margin-right: 12px;
        }}
        .button {{
            display: inline-block;
            padding: 16px 38px;
            background: linear-gradient(90deg, #7f5cff 0%, #00e6f6 100%);
            color: #fff;
            font-family: 'Press Start 2P', 'Consolas', monospace;
            font-size: 1rem;
            text-decoration: none;
            border-radius: 30px;
            margin: 32px 0 18px 0;
            box-shadow: 0 4px 16px #00e6f666;
            letter-spacing: 1px;
            transition: background 0.2s;
        }}
        .button:hover {{
            background: linear-gradient(90deg, #00e6f6 0%, #7f5cff 100%);
        }}
        .footer {{
            text-align: center;
            font-size: 0.95rem;
            color: #fff;
            background: #1a1c2b;
            padding: 18px 10px 10px 10px;
        }}
        .footer strong {{
            color: #00e6f6;
        }}
        @media (max-width: 700px) {{
            .container {{
                margin: 0;
                border-radius: 0;
            }}
            .content {{
                padding: 18px 8px 18px 8px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <span class='gamer-icon'>🎮</span>
            <h1>¡Bienvenido a la Comunidad Gamer!</h1>
        </div>
        <div class='content'>
            <h2>¡Hola, leyenda!</h2>
            <p>Te damos la bienvenida a <b>PixelVault</b>, donde los verdaderos gamers se reúnen.<br>
            Prepárate para subir de nivel y desbloquear recompensas épicas.</p>
            <ul class='features'>
                <li><span class='emoji'>📰</span> Noticias y actualizaciones de tus juegos favoritos</li>
                <li><span class='emoji'>🎁</span> Loot boxes y ofertas exclusivas solo para miembros</li>
                <li><span class='emoji'>🚀</span> Acceso anticipado a lanzamientos y betas</li>
                <li><span class='emoji'>🏆</span> Torneos, rankings y logros especiales</li>
                <li><span class='emoji'>💬</span> Comunidad activa para compartir tus mejores jugadas</li>
            </ul>
            <center>
                <a href='https://www.linkedin.com/in/adrian-castro-9b7712256/' class='button'>¡Éntra a mi LinkedIn!</a>
            </center>
            <p style='margin-top:32px;'>¿Tienes dudas o sugerencias? ¡Respóndenos y únete a la conversación!</p>
            <p style='margin-top:18px;'>¡Gracias por unirte a la familia PixelVault!<br>
            <strong>El Equipo PixelVault</strong></p>
        </div>
        <div class='footer'>
            <p>Este email fue enviado a <strong>{email}</strong></p>
            <p>Si no te suscribiste, puedes ignorar este mensaje.</p>
        </div>
    </div>
</body>
</html>";
        }


    }
}