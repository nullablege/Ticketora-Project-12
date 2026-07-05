using Application.Abstract.Service;
using Application.DTOs.Profile;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class ProfileService : IProfileService
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ProfileDto?> GetProfileAsync(int userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return null;
        }

        return new ProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    public async Task<ProfileOperationResult> UpdateProfileAsync(UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Kullanıcı bulunamadı."
            };
        }

        user.FullName = request.FullName;
        user.Email = request.Email;
        user.UserName = request.Email;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return CreateFailedResult("Profil bilgileri güncellenemedi.", result);
        }

        return new ProfileOperationResult
        {
            Succeeded = true,
            Message = "Profil bilgileriniz güncellendi."
        };
    }

    public async Task<ProfileOperationResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        if (request.CurrentPassword == null || request.CurrentPassword == "")
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Mevcut şifrenizi girmelisiniz."
            };
        }

        if (request.NewPassword == null || request.NewPassword == "")
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Yeni şifrenizi girmelisiniz."
            };
        }

        if (request.ConfirmNewPassword == null || request.ConfirmNewPassword == "")
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Yeni şifre onayını girmelisiniz."
            };
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Yeni şifreler eşleşmiyor."
            };
        }

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
        {
            return new ProfileOperationResult
            {
                Succeeded = false,
                Message = "Kullanıcı bulunamadı."
            };
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return CreateFailedResult("Şifre değiştirilemedi.", result);
        }

        return new ProfileOperationResult
        {
            Succeeded = true,
            Message = "Şifreniz güncellendi."
        };
    }

    private ProfileOperationResult CreateFailedResult(string message, IdentityResult identityResult)
    {
        var result = new ProfileOperationResult
        {
            Succeeded = false,
            Message = message
        };

        foreach (var error in identityResult.Errors)
        {
            result.Errors.Add(error.Description);
        }

        return result;
    }
}
