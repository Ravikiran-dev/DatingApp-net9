namespace API.DTO;

public class PhotoDto
{
    public int Id { get; set; }
    public string? Url { get; set; } // Initialize to null to avoid null reference issues
    public bool IsMain { get; set; } // Initialize to null to avoid null reference issues
}