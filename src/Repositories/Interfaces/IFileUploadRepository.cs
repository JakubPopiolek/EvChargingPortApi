using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Repositories
{
    public interface IFileUploadRepository
    {
        UploadedFile GetUploadedFile(long id);
        void InsertUploadedFile(UploadedFile uploadedFile);
        void Save();
    }
}
