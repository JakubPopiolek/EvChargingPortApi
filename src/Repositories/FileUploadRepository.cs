using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvApplicationApi.Helpers;

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
