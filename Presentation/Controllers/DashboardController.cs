using Application.Abstract.Service;
using Application.DTOs.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels.Dashboard;
using System.Security.Claims;

namespace Presentation.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IProfileService _profileService;

    public DashboardController(IDashboardService dashboardService, IProfileService profileService)
    {
        _dashboardService = dashboardService;
        _profileService = profileService;
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);
        var summary = await _dashboardService.GetDashboardSummaryAsync(userId);
        var pastEvents = await _dashboardService.GetPastEventsAsync(userId);

        var model = new DashboardIndexViewModel
        {
            Summary = summary,
            PastEvents = pastEvents,
            FullName = GetFullName(profile),
            Email = GetEmail(profile)
        };

        return View(model);
    }

    public async Task<IActionResult> Tickets(int? ticketId)
    {
        var userId = GetCurrentUserId();
        var profile = await _profileService.GetProfileAsync(userId);
        var activeTickets = await _dashboardService.GetActiveTicketsAsync(userId);
        var selectedTicket = ticketId.HasValue
            ? await _dashboardService.GetTicketDetailAsync(ticketId.Value, userId)
            : null;

        var model = new DashboardTicketsViewModel
        {
            ActiveTickets = activeTickets,
            SelectedTicket = selectedTicket,
            FullName = GetFullName(profile),
            Email = GetEmail(profile)
        };

        return View(model);
    }

    public async Task<IActionResult> Settings()
    {
        var profile = await _profileService.GetProfileAsync(GetCurrentUserId());

        var model = new DashboardSettingsViewModel
        {
            FullName = GetFullName(profile),
            Email = GetEmail(profile)
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(DashboardSettingsViewModel model)
    {
        var userId = GetCurrentUserId();
        var result = await _profileService.UpdateProfileAsync(new UpdateProfileRequest
        {
            UserId = userId,
            FullName = model.FullName,
            Email = model.Email
        });

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", result.Message);

            return View(model);
        }

        if (PasswordChangeRequested(model))
        {
            var passwordResult = await _profileService.ChangePasswordAsync(new ChangePasswordRequest
            {
                UserId = userId,
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword,
                ConfirmNewPassword = model.ConfirmNewPassword
            });

            if (!passwordResult.Succeeded)
            {
                ModelState.AddModelError("", passwordResult.Message);

                return View(model);
            }

            ViewData["ProfileMessage"] = "Profil bilgileriniz ve şifreniz güncellendi.";

            return View(model);
        }

        ViewData["ProfileMessage"] = result.Message;

        return View(model);
    }

    private int GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        int.TryParse(userId, out int id);

        return id;
    }

    private string GetCurrentUserName()
    {
        var name = User.Identity?.Name;

        if (name == null)
        {
            return "";
        }

        return name;
    }

    private string GetCurrentUserEmail()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
        {
            return GetCurrentUserName();
        }

        return email;
    }

    private string GetFullName(ProfileDto? profile)
    {
        if (profile == null)
        {
            return GetCurrentUserName();
        }

        return profile.FullName;
    }

    private string GetEmail(ProfileDto? profile)
    {
        if (profile == null)
        {
            return GetCurrentUserEmail();
        }

        return profile.Email;
    }

    private bool PasswordChangeRequested(DashboardSettingsViewModel model)
    {
        if (model.CurrentPassword != null && model.CurrentPassword != "")
        {
            return true;
        }

        if (model.NewPassword != null && model.NewPassword != "")
        {
            return true;
        }

        if (model.ConfirmNewPassword != null && model.ConfirmNewPassword != "")
        {
            return true;
        }

        return false;
    }
}
