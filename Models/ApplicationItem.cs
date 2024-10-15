namespace EvApplicationApi.Models;

public class ApplicationItem
{
    public long Id { get; set; }
    public required string Vrn { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Address { get; set; }
    public string? ApplicationStatus { get; set; }
    public string? ApplicationOutcome { get; set; }
}
