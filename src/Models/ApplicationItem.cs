namespace EvApplicationApi.Models;

public class ApplicationItem
{
    public long Id { get; set; }
    public Guid ReferenceNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
    public required string Vrn { get; set; }
    public ICollection<UploadedFile> Files { get; } = new List<UploadedFile>();
}
