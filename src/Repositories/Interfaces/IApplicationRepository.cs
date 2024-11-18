using EvApplicationApi.DTOs;
using EvApplicationApi.Models;

namespace EvApplicationApi.Repositories.Interfaces
{
    public interface IApplicationRepository : IDisposable
    {
        void SubmitApplication(ApplicationItem applicationItem);
        Guid StartApplication();
        Task<ApplicationItemDto?> GetApplicationItemDto(Guid referenceNumber);
        Task<ApplicationItem?> GetApplicationItem(Guid referenceNumber);
        void Save();
    }
}
