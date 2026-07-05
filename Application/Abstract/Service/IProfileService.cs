using Application.DTOs.Profile;

namespace Application.Abstract.Service;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync(int userId);
    Task<ProfileOperationResult> UpdateProfileAsync(UpdateProfileRequest request);
    Task<ProfileOperationResult> ChangePasswordAsync(ChangePasswordRequest request);
}
