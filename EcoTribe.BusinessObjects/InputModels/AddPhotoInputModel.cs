using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EcoTribe.BusinessObjects.InputModels
{
    public class AddPhotoInputModel
    {
        public int EventId { get; set; }
        public IFormFile Photo { get; set; } = null!;
    }
}
