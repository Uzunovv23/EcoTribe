using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class EventPhotoViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public DateTime UploadedOn { get; set; }
    }
}
