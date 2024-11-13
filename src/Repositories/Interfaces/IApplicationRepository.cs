using EvApplicationApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EvApplicationApi.Repositories.Interfaces
{
    public interface IApplicationRepository : IDisposable
    {
        void SubmitApplication(ApplicationItem applicationItem);
        Guid BeginApplication();
        ApplicationItem GetApplicationItem(Guid id);
        void Save();
    }
}
