using EvApplicationApi.DTOs;
using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace src.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private ApplicationContext context;

        public FileUploadRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<bool> DeleteFileAsync(long id)
        {
            var file = await context.UploadedFiles.FindAsync(id);
            if (file == null)
                return false;

            context.Remove(file);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAllFilesInApplication(Guid referenceNumber)
        {
            var uploadedFiles = await context
                .UploadedFiles.Where(item => item.ApplicationReferenceNumber == referenceNumber)
                .ToListAsync();

            uploadedFiles.ForEach(file =>
            {
                context.Remove(file);
            });
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<List<UploadedFileDto>> GetUploadedFiles(Guid referenceNumber)
        {
            var uploadedFiles = await context
                .UploadedFiles.Where(item => item.ApplicationReferenceNumber == referenceNumber)
                .ToListAsync();

            List<UploadedFileDto> outputFiles = [];

            uploadedFiles.ForEach(file =>
            {
                UploadedFileDto fileDto = new UploadedFileDto() { Id = file.Id, Name = file.Name };
                outputFiles.Add(fileDto);
            });
            return outputFiles;
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
