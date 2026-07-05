namespace Application.DTOs.Profile;

public class UpdateProfileRequest
{
    public int UserId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}
