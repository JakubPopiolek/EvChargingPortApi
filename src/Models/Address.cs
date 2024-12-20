using System.Text.Json.Serialization;

namespace EvApplicationApi.Models;

public class Address
{
    public long Id { get; set; }
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Postcode { get; set; }

    [JsonIgnore]
    public ApplicationItem? ApplicationItem { get; set; }
}
