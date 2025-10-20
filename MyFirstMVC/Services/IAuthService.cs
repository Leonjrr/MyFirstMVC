using MyFirstMVC.Models;

namespace MyFirstMVC.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<TwoFactorResponse> VerifyTwoFactorAsync(TwoFactorRequest request);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    }
}
