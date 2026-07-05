namespace Application.DTOs.Profile;

public class ProfileOperationResult
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string> Errors { get; set; } = new();
}
