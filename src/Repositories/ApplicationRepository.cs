using EvApplicationApi.DTOs;
using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EvApplicationApi.Repository
{
    public class ApplicationRepository : IApplicationRepository, IDisposable
    {
        private ApplicationContext context;

        public ApplicationRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<ApplicationItem?> GetApplicationItem(Guid referenceNumber)
        {
            return await context
                .ApplicationItems.Include(ai => ai.Address)
                .Include(ai => ai.Files)
                .FirstOrDefaultAsync(ai => ai.ReferenceNumber == referenceNumber);
        }

        public async Task<ApplicationItemDto?> GetApplicationItemPublic(Guid referenceNumber)
        {
            var applicationItem = await context
                .ApplicationItems.Include(ai => ai.Address)
                .Include(ai => ai.Files)
                .FirstOrDefaultAsync(ai => ai.ReferenceNumber == referenceNumber);

            if (applicationItem == null)
            {
                return null;
            }

            var applicationItemDto = new ApplicationItemDto
            {
                ReferenceNumber = applicationItem.ReferenceNumber,
                FirstName = applicationItem.FirstName,
                LastName = applicationItem.LastName,
                Email = applicationItem.Email,
                Address = new AddressDto()
                {
                    Line1 = applicationItem.Address?.Line1,
                    Line2 = applicationItem.Address?.Line2,
                    City = applicationItem.Address?.City,
                    Province = applicationItem.Address?.Province,
                    Postcode = applicationItem.Address?.Postcode,
                },
                Vrn = applicationItem.Vrn,
                Files = applicationItem
                    .Files.Select(f => new UploadedFileDto { Id = f.Id, Name = f.Name })
                    .ToList(),
            };
            return applicationItemDto;
        }

        public Guid StartApplication()
        {
            Guid referenceNumber = Guid.NewGuid();
            context.ApplicationItems.Add(
                new ApplicationItem() { ReferenceNumber = referenceNumber }
            );
            return referenceNumber;
        }

        public void SubmitApplication(ApplicationItem applicationItem)
        {
            context.ApplicationItems.Update(applicationItem);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
