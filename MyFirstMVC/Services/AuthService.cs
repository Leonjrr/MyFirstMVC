using Microsoft.Extensions.Configuration;
using MyFirstMVC.Models;
using System.Net.Http;
using System.Text.Json;

namespace MyFirstMVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        public AuthService ( HttpClient httpClient, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        public async Task<LoginResponse> LoginAsync (LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://207.180.246.69:9010/api/accounts/login", request);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Raw API Response: {Content}", content);

                if (response.IsSuccessStatusCode)
                {
                    var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper<LoginResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (wrapper != null && wrapper.IsSuccessful && wrapper.Data != null)
                    {
                        _logger.LogInformation("Login successful for {Email}. 2FA Required: {2FA}",
                            request.Email, wrapper.Data.TwoFactorRequired);

                        return wrapper.Data;
                    }
                    else
                    {
                        return new LoginResponse
                        {
                            Success = false,
                            Message = wrapper?.Message ?? "Login failed: Invalid response format"
                        };
                    }
                }
                else
                {
                    // Handle HTTP error status
                    try
                    {
                        var errorWrapper = JsonSerializer.Deserialize<ApiResponseWrapper<object>>(content,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        return new LoginResponse
                        {
                            Success = false,
                            Message = errorWrapper?.Message ?? $"Login failed with status: {response.StatusCode}"
                        };
                    }
                    catch
                    {
                        return new LoginResponse
                        {
                            Success = false,
                            Message = $"Login failed with status: {response.StatusCode}"
                        };
                    }
                }

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "login failed for {Email}", request.Email);
                return new LoginResponse
                {
                    Success = false,
                    Message = $"Login Error: {ex.Message}"
                };
            }
        }

        public async Task<TwoFactorResponse> VerifyTwoFactorAsync(TwoFactorRequest request)
        {
            try
            {
                // Log the request we're sending
                var requestJson = JsonSerializer.Serialize(request);
                _logger.LogInformation("Sending 2FA request: {Request}", requestJson);

                var response = await _httpClient.PostAsJsonAsync("http://207.180.246.69:9010/api/accounts/verifyotp", request);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("2FA Raw API Response: {Content}", content);
                _logger.LogInformation("2FA Status Code: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var otpResponse = JsonSerializer.Deserialize<TwoFactorResponse>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (otpResponse != null)
                    {
                        _logger.LogInformation("2FA verification successful for {Email}", request.Email);
                        return otpResponse;
                    }
                    else
                    {
                        return new TwoFactorResponse
                        {
                            Success = false,
                        };
                    }
                }

                else
                {
                    var errorResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(content);
                    var errorMessage = errorResponse?.ContainsKey("message") == true
                    ? errorResponse["message"]?.ToString()
                    : $"2FA verification failed with status: {response.StatusCode}";
                    return new TwoFactorResponse
                    {
                        Success = false
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "2FA verification failed for {Email}", request.Email);
                return new TwoFactorResponse
                {
                    Success = false,
                };
            }
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("http://207.180.246.69:9010/api/accounts/passwordreset", request);
                var content = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Forgot Password Raw API Response: {Content}", content);

                if (response.IsSuccessStatusCode)
                {
                    var wrapper = JsonSerializer.Deserialize<ApiResponseWrapper<bool>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (wrapper != null && wrapper.IsSuccessful)
                    {
                        return new ForgotPasswordResponse
                        {
                            Data = wrapper.Data,
                            Message = wrapper.Message,
                            IsSuccessful = true,
                            StatusCode = wrapper.StatusCode
                        };
                    }

                    else
                    {
                        return new ForgotPasswordResponse
                        {
                            IsSuccessful = false,
                            Message = wrapper?.Message ?? "Password reset failed"
                        };
                    }
                }

                else
                {
                    var errorWrapper = JsonSerializer.Deserialize<ApiResponseWrapper<object>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return new ForgotPasswordResponse
                    {
                        IsSuccessful = false,
                        Message = errorWrapper?.Message ?? $"Password reset failed with status: {response.StatusCode}"
                    };
                }
            }
            
            catch(Exception ex)
            {
                _logger.LogError(ex, "Password Reset failed for {Email}", request.Email);
                return new ForgotPasswordResponse
                {
                    IsSuccessful = false, 
                    Message = $"Password reset error: {ex.Message}"
                };
            }

        }
    }
}
