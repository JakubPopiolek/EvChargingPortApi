using EvApplicationApi.Helpers;
using EvApplicationApi.Models;

namespace EvApplicationApi.Repository
{
    public class ApplicationRepository : IApplicationsRepository, IDisposable
    {
        private ApplicationContext context;

        public ApplicationRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public ApplicationItem GetApplicationItem(long id)
        {
            return context.ApplicationItems.Find(id);
        }

        public void InsertApplication(ApplicationItem applicationItem)
        {
            context.ApplicationItems.Add(applicationItem);
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
