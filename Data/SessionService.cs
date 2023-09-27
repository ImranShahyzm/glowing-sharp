using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using System.Text.Json;
using System.Text;

namespace SagErpBlazor.Data
{

    public class SessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDataProtector _dataProtector;

        public SessionService(IHttpContextAccessor httpContextAccessor, IDataProtectionProvider dataProtectionProvider)
        {
            _httpContextAccessor = httpContextAccessor;

            // Use a purpose string specific to your application to isolate encryption/decryption keys
            _dataProtector = dataProtectionProvider.CreateProtector("MyUserDetails");
        }

        public async Task SaveAuthenticationStateAsync(AuthenticationState authState)
        {
            var customAuthState = new CustomClaims
            {
                Claims = authState.User.Claims.ToList()
            };

            var serializedAuthState = JsonSerializer.Serialize(customAuthState);

            // Encrypt the serialized authentication state before storing it in session
            var protectedData = _dataProtector.Protect(Encoding.UTF8.GetBytes(serializedAuthState));
            _httpContextAccessor.HttpContext.Session.Set("AuthState", protectedData);
            await Task.CompletedTask;
        }

        public Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var protectedData = _httpContextAccessor.HttpContext.Session.Get("AuthState");
            if (protectedData == null || protectedData.Length == 0)
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));

            // Decrypt the protected data and deserialize the authentication state
            var serializedAuthState = _dataProtector.Unprotect(protectedData);
            var customAuthState = JsonSerializer.Deserialize<CustomClaims>(Encoding.UTF8.GetString(serializedAuthState));

            // Create an AuthenticationState instance using the custom model
            var claimsIdentity = new ClaimsIdentity(customAuthState.Claims, "custom-auth-type");
            var user = new ClaimsPrincipal(claimsIdentity);
            var authState = new AuthenticationState(user);

            return Task.FromResult(authState);
        }
    }

}