using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.Config
{
    public class UploadSettings
    {
        public string RootPath { get; set; } = string.Empty;      
        public string EventsPath { get; set; } = "events";         
        public string VolunteersPath { get; set; } = "volunteers"; 
        public string OrganizationsPath { get; set; } = "organizations";
    }
}
