using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
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
        public async Task<IActionResult> Post(
            List<IFormFile> files,
            Guid applicationReferenceNumber
        )
        {
            if (applicationReferenceNumber == Guid.Empty)
            {
                return BadRequest("Missing Guid");
            }

            long size = files.Sum(f => f.Length);

            var filePath = Path.GetTempFileName();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        // Upload the file if less than 2 MB
                        if (stream.Length < 2097152)
                        {
                            var fileToUpload = new UploadedFile()
                            {
                                Data = stream.ToArray(),
                                Name = file.FileName,
                                ApplicationReferenceNumber = applicationReferenceNumber,
                            };

                            _fileUploadRepository.InsertUploadedFile(fileToUpload);
                            _fileUploadRepository.Save();
                        }
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }
    }
}
