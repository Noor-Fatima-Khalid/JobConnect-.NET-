using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models
{
    public class ApplyViewModel
    {
        public int JobId { get; set; }
        //public Job Job { get; set; }
        [Required]
        public IFormFile ResumeFile { get; set; }
        public Resume Resume { get; set; } = new Resume();
    }
}
