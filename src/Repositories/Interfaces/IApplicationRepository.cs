using EvApplicationApi.Models;

namespace EvApplicationApi.Repository.Interfaces
{
    public interface IApplicationRepository : IDisposable
    {
        ApplicationItem GetApplicationItem(long id);
        void InsertApplication(ApplicationItem applicationItem);
        void Save();
    }
}
