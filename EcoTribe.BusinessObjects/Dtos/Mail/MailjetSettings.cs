using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Dtos.Mail
{
    public class MailjetSettings
    {
        public string ApiKeyPublic { get; set; }
        public string ApiKeyPrivate { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}
