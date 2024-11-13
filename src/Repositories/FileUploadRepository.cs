using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;

namespace src.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private ApplicationContext context;

        public FileUploadRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public UploadedFile GetUploadedFile(long id)
        {
            return context.UploadedFiles.Find(id)!;
        }

        public void InsertUploadedFile(UploadedFile uploadedFile)
        {
            context.UploadedFiles.Add(uploadedFile);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
