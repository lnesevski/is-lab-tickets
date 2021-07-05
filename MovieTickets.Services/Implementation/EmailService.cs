using MailKit.Security;
using MimeKit;
using MovieTickets.Domain;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Services.Interface;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(EmailSettings settings)
        {
            this._settings = settings;
        }

        public async Task SendEmailAsync(List<EmailMessage> emailMessages)
        {
            List<MimeMessage> mimeMessages = new List<MimeMessage>();

            foreach (var item in emailMessages)
            {
                var emailMessage = new MimeMessage
                {
                    Sender = new MailboxAddress(_settings.SenderName, _settings.SmtpUsername),
                    Subject = item.Subject

                };
                emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUsername));

                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = item.Content};

                emailMessage.To.Add(new MailboxAddress(item.MailTo));

                mimeMessages.Add(emailMessage);
            }

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOption = _settings.EnableSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;

                    await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpServerPort, socketOption);
                    
                    if(!string.IsNullOrEmpty(_settings.SmtpUsername))
                    {
                        await smtp.AuthenticateAsync(_settings.SmtpUsername, _settings.SmtpPassword);
                    }

                    foreach (var item in mimeMessages)
                    {
                        await smtp.SendAsync(item);
                    }

                    await smtp.DisconnectAsync(true);
                }
            }
             catch (SmtpException exception)
            {
                throw exception;
            }
        }
    }
}
