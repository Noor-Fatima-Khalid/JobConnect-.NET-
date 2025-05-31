using Microsoft.AspNetCore.Identity;

namespace JobPortal.Models
{
    public class SavedJobs
    {
        public int Id { get; set; }

        public int JobId { get; set; }      //FK
        public Job? Job { get; set; }        // nav prop
    }
}
