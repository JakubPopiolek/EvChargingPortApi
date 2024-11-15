using EvApplicationApi.DTOs;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace EvApplicationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadRepository _fileUploadRepository;

        private readonly IConfiguration _configuration;

        public FileUploadController(
            IFileUploadRepository fileUploadRepository,
            IConfiguration configuration
        )
        {
            _fileUploadRepository = fileUploadRepository;
            _configuration = configuration;
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

        [HttpGet]
        [Route("getFileExtensions")]
        public ActionResult<string[]> GetAllowedExtensions()
        {
            var permittedExtensions = _configuration
                .GetSection("PermittedFileUploadExtensions")
                .Get<string[]>();
            return Ok(permittedExtensions);
        }

        [HttpPost("{applicationReference}")]
        [EnableRateLimiting("uploadFile_fixed")]
        public async Task<IActionResult> Post(List<IFormFile> files, Guid applicationReference)
        {
            if (applicationReference == Guid.Empty)
            {
                return BadRequest("Missing Guid");
            }

            var permittedExtensions = new[] { ".jpg", ".png", ".pdf", ".doc", ".docx", ".txt" };

            List<UploadedFileDto> uploadedFiles = [];

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    var allowedExtensionsList = string.Join(" ", permittedExtensions);
                    return UnprocessableEntity(
                        $"Invalid file type. File must be one of: {allowedExtensionsList}."
                    );
                }
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
                    else
                    {
                        return UnprocessableEntity("File is too large");
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
