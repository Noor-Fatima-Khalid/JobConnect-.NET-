namespace JobPortal.Models
{
    public class Applicant
    {
        public int Id { get; set; }
        public string UserId { get; set; }              // FK
        public User User { get; set; }                 // nav property

        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Skill { get; set; }
        public string? TaglineSkills { get; set; }       // web dev / ML engineer etc etc. (must write spearated by comma)
        public string? About { get; set; }               // a short summary about the skills applicants have and professional acheivements
        public string? Education { get; set; }          // format: Degree, Institute
        //public List<WorkExperience>? Experience { get; set; } = new List<WorkExperience>();          // format: position, company, duration
        public string? Certifications { get; set; }


        // navigation prop
        public List<JobApplication> JobApplications { get; set; } = new();
        public List<Resume> Resumes { get; set; } = new();
    }
}