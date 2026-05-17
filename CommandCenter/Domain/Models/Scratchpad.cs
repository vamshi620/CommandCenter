namespace CommandCenter.Domain.Models;

/// <summary>A named scratchpad pad stored in the database.</summary>
public class Scratchpad
{
    public int    Id          { get; set; }
    public string Title       { get; set; } = "Untitled";
    public string Content     { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
}
