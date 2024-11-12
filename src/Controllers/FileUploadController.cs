using Microsoft.AspNetCore.Mvc;
using src.Repositories;

namespace EvApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadRepository _fileUploadRepository;

        public FileUploadController(IFileUploadRepository fileUploadRepository)
        {
            _fileUploadRepository = fileUploadRepository;
        }
    }
}
