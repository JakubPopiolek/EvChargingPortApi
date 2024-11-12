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

        [HttpGet("{id}")]
        public ActionResult<UploadedFile> GetUploadedFile(long id)
        {
            var uploadedFile = _fileUploadRepository.GetUploadedFile(id);

            if (uploadedFile == null)
            {
                return NotFound();
            }

            return Ok(uploadedFile);
        }

        [HttpPost]
        public ActionResult<UploadedFile> UploadFile(UploadedFile uploadedFile)
        {
            _fileUploadRepository.InsertUploadedFile(uploadedFile);
            _fileUploadRepository.Save();

            return CreatedAtAction("GetUploadedFile", new { id = uploadedFile.Id }, uploadedFile);
        }
    }
}
