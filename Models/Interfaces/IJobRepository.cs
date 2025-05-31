namespace JobPortal.Models.Interfaces
{
    public interface IJobRepository
    {
        //Task<List<Job>> GetAllForFeedAsync();
        Task<List<Job>> GetAllAsync();
        Task<List<Job>> GetJobsAsync(string userId);
        Task<List<Job>> GetLatestJobsAsync(string userId, int count);
        Task<Job?> GetByIdAsync(int id);
        Task<Job> AddJob(Job job);
        Task<Job?> UpdateJob(Job job);
        Task DeleteJob(int id);
    }

}
