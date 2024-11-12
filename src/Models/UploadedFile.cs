using EvApplicationApi.Models;

public class UploadedFile
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Data { get; set; }
    public ApplicationItem? ApplicationItem { get; set; }
}
