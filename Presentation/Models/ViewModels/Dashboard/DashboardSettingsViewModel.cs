namespace Presentation.Models.ViewModels.Dashboard;

public class DashboardSettingsViewModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
}
