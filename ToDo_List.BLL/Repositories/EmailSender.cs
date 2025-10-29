using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ToDo_List.BLL.Interfaces;

namespace ToDo_List.BLL.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_config["Email:Address"], _config["Email:Password"]),
                EnableSsl = true,
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_config["Email:Address"], "Taskora"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mail.To.Add(email);

            await client.SendMailAsync(mail);
        }
    }

}
