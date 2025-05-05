using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entity;

[Table("Photos")]
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; } 
    public string? PublicId { get; set; } 
    public bool IsMain { get; set; } 

    // Navigation property to the AppUser entity
    public int AppUserId { get; set; } 
    public AppUser AppUser { get; set; } = null!; // Initialize to null to avoid null reference issues
}