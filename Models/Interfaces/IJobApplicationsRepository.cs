namespace JobPortal.Models.Interfaces
{
    public interface IJobApplicationsRepository
    {
        Task<List<JobApplication>> GetbyAppIdAsync(int appId);
    }
}
