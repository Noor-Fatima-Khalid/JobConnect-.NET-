using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Models.Interfaces
{
    public interface IApplicantRepository
    {
        Task<Applicant> GetById(string id);
        Task UpdateProfileAsync(Applicant applicant, EditProfileViewModel model);
    }
}
