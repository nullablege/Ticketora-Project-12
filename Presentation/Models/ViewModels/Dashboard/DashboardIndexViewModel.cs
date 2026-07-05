using Application.DTOs.Dashboard;

namespace Presentation.Models.ViewModels.Dashboard;

public class DashboardIndexViewModel
{
    public DashboardSummaryDto Summary { get; set; }
    public List<PastEventDto> PastEvents { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
}
