using EvApplicationApi.Models;

namespace EvApplicationApi.Repositories.Interfaces
{
    public interface IFileUploadRepository
    {
        UploadedFile GetUploadedFile(long id);
        void InsertUploadedFile(UploadedFile uploadedFile);
        void Save();
    }
}
