using Microsoft.EntityFrameworkCore;

public class Address
{
    public long Id { get; set; }
    public required string Line1 { get; set; }
    public string? Line2 { get; set; }
    public required string City { get; set; }
    public string? Province { get; set; }
    public required string Postcode { get; set; }
}
