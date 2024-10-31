namespace EvApplicationApi.Models;

public class ApplicationItem
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required Address Address { get; set; }
    public required string Vrn { get; set; }
}
