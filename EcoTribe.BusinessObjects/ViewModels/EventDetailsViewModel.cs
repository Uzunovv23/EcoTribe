using EcoTribe.BusinessObjects.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.BusinessObjects.ViewModels
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public int RequiredVolunteers { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public List<VolunteerViewModel> AttendingVolunteers { get; set; } = new List<VolunteerViewModel>();
        public List<EventSponsorViewModel> Sponsors { get; set; } = new List<EventSponsorViewModel>();
        public List<FeedbackViewModel> Feedbacks { get; set; } = new List<FeedbackViewModel>();
        public FeedbackInputModel FeedbackInput { get; set; } = new FeedbackInputModel();

    }
}
