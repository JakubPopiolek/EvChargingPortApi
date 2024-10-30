using EvApplicationApi.Models;

namespace EvApplicationApi.Repository
{
    public interface IApplicationsRepository : IDisposable
    {
        ApplicationItem GetApplicationItem(long id);
        void InsertApplication(ApplicationItem applicationItem);
        void Save();
    }
}