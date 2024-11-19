using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EvApplicationApi.Models;

public class ApplicationItem
{
    [Key]
    public Guid ReferenceNumber { get; set; }
    public long? AddressId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public Address? Address { get; set; }
    public string? Vrn { get; set; }
    public ICollection<UploadedFile> Files { get; } = new List<UploadedFile>();

    [JsonIgnore]
    public DateTime? Timestamp { get; set; }
}
