using EvApplicationApi.DTOs;
using EvApplicationApi.Models;

namespace EvApplicationApi.Repositories.Interfaces
{
    public interface IFileUploadRepository
    {
        Task<List<UploadedFileDto>> GetUploadedFiles(Guid id);
        Task<bool> DeleteFileAsync(long id);
        Task<bool> DeleteAllFilesInApplication(Guid referenceNumber);
        void InsertUploadedFile(UploadedFile uploadedFile);
        void Save();
    }
}
