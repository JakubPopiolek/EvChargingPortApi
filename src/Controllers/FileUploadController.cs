using EvApplicationApi.DTOs;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<ActionResult<UploadedFile[]>> GetUploadedFiles(Guid id)
        {
            var uploadedFiles = await _fileUploadRepository.GetUploadedFiles(id);

            if (uploadedFiles.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(uploadedFiles);
        }

        [HttpPost("{applicationReference}")]
        public async Task<IActionResult> Post(List<IFormFile> files, Guid applicationReference)
        {
            if (applicationReference == Guid.Empty)
            {
                return BadRequest("Missing Guid");
            }

            List<UploadedFileDto> uploadedFiles = [];

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);

                    // Upload the file if less than 2 MB
                    if (stream.Length < 2097152)
                    {
                        var fileToUpload = new UploadedFile()
                        {
                            Data = stream.ToArray(),
                            Name = file.FileName,
                            ApplicationReferenceNumber = applicationReference,
                        };

                        _fileUploadRepository.InsertUploadedFile(fileToUpload);
                        _fileUploadRepository.Save();

                        UploadedFileDto fileToUploadDto = new UploadedFileDto()
                        {
                            Id = fileToUpload.Id,
                            Name = fileToUpload.Name,
                        };

                        uploadedFiles.Add(fileToUploadDto);
                    }
                }
            }
            return Ok(uploadedFiles);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFile(long id)
        {
            var success = await _fileUploadRepository.DeleteFileAsync(id);
            if (!success)
            {
                return NotFound(new { Message = "Entity not found." });
            }
            return NoContent();
        }
    }
}
