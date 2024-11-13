using EvApplicationApi.Helpers;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EvApplicationApi.Repository
{
    public class ApplicationRepository : IApplicationRepository, IDisposable
    {
        private ApplicationContext context;

        public ApplicationRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public ApplicationItem GetApplicationItem(Guid id)
        {
            return context.ApplicationItems.Find(id)!;
        }

        public Guid BeginApplication()
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
