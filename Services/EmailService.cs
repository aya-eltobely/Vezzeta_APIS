
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
//using System.Net.Mail;
using VezetaApi.Interfaces;
using MailKit.Net.Smtp;
using Org.BouncyCastle.Crypto.Macs;

namespace VezetaApi.Services
{
    public class EmailService : IEmailService
    {
        public void SendWelcomeEmail(string toEmail, string userName, string password)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Aya Eltobely", "aya.ay@gmail.com"));
            message.To.Add(new MailboxAddress(userName, toEmail));
            message.Subject = "Doctor Created Succesfully";
            message.Body = new TextPart("plain")
            {
                Text = $"Dear {userName},\\n\\nWelcome to Vezzeta App! , your Account Created and Your User Name  {userName} : Password {password}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("sandbox.smtp.mailtrap.io", 25, false);
                client.Authenticate("1c4b2ea3ef4eb1", "2a5bec086f71d9");

                client.Send(message);
                client.Disconnect(true);
            }

         


        }
    }
}

