using EvApplicationApi.Models;

namespace EvApplicationApi.DTOs;

public class ApplicationItemDto
{
    public Guid ReferenceNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public AddressDto? Address { get; set; }
    public string? Vrn { get; set; }
    public ICollection<UploadedFileDto> Files { get; set; } = new List<UploadedFileDto>();
}
