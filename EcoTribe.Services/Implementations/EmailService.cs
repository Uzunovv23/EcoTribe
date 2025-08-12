using EcoTribe.BusinessObjects.Dtos.Mail;
using EcoTribe.Services.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcoTribe.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly MailjetSettings _mailjetSettings;

        public EmailService(IOptions<MailjetSettings> mailjetSettings)
        {
            _mailjetSettings = mailjetSettings.Value;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var client = new MailjetClient(
                _mailjetSettings.ApiKeyPublic,
                _mailjetSettings.ApiKeyPrivate);

            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, _mailjetSettings.SenderEmail)
            .Property(Send.FromName, _mailjetSettings.SenderName)
            .Property(Send.Subject, subject)
            .Property(Send.TextPart, body)
            .Property(Send.HtmlPart, $"<h3>{body}</h3>")
            .Property(Send.Recipients, new JArray {
        new JObject {
            { "Email", to }
        }
            });

            var response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Mailjet error: {response.GetErrorMessage()}");
            }
        }

    }
}
