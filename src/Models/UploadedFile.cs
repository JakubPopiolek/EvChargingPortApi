namespace EvApplicationApi.Models;

public class UploadedFile
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required byte[] Data { get; set; }
    public required Guid ApplicationReferenceNumber { get; set; }
    public ApplicationItem? ApplicationItem { get; set; }
}
