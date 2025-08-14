using EcoTribe.BusinessObjects.Dtos.Mail;
using EcoTribe.Services.Interfaces;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EcoTribe.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly MailjetSettings _mailjetSettings;

        public EmailService(IOptions<MailjetSettings> mailjetSettings)
        {
            _mailjetSettings = mailjetSettings.Value;
        }

        /// <summary>
        /// Sends an HTML email via Mailjet
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="htmlBody">Email body in HTML format</param>
        public async Task SendEmail(string to, string subject, string htmlBody)
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
            .Property(Send.HtmlPart, htmlBody)
            .Property(Send.TextPart, StripHtmlTags(htmlBody)) // fallback for plain text
            .Property(Send.Recipients, new JArray
            {
                new JObject
                {
                    { "Email", to }
                }
            });

            var response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Mailjet error: {response.GetErrorMessage()}");
            }
        }

        /// <summary>
        /// Removes HTML tags for plain-text fallback
        /// </summary>
        private string StripHtmlTags(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
