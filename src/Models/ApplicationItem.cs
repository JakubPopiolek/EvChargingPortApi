using System.ComponentModel.DataAnnotations;

namespace EvApplicationApi.Models;

public class ApplicationItem
{
    [Key]
    public Guid ReferenceNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Address? Address { get; set; }
    public string? Vrn { get; set; }
    public ICollection<UploadedFile> Files { get; } = new List<UploadedFile>();
}
