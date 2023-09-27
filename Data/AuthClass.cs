using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SagErpBlazor.Services;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace SagErpBlazor.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorageService { get; }
        public IUserService _userService { get; set; }
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService,
            IUserService userService,
            HttpClient httpClient)
        {
            //throw new Exception("CustomAuthenticationStateProviderException");
            _localStorageService = localStorageService;
            _userService = userService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");

            ClaimsIdentity identity;

            if (accessToken != null && accessToken != string.Empty)
            {
                GLUser user = await _userService.GetUserByAccessTokenAsync(accessToken);
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAsAuthenticated(GLUser user)
        {
            await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
            await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await _localStorageService.RemoveItemAsync("refreshToken");
            await _localStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            ActiveUserSingleton.Logout();
        }

        private ClaimsIdentity GetClaimsIdentity(GLUser user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Username != null)
            {
               
                    claimsIdentity = new ClaimsIdentity(new[]
                                    {
                                    new Claim(ClaimTypes.Name, user.Username),
                                    new Claim(ClaimTypes.Role, user.Role.RoleDescription),
                                    new Claim("FirstName", user.FirstName),
                                    new Claim("LastName", user.LastName),
                                    new Claim("CompanyID", Convert.ToString(user.CompanyID)),
                                    new Claim("Userid", Convert.ToString(user.Userid)),
                                    new Claim(ClaimTypes.Surname, "nav-function-top"),
                                    new Claim("SupportStaffName", user._StsSupportStaff.Name??""),
                                    new Claim("SupportStaffId",  Convert.ToString(user._StsSupportStaff.SupportStaffId)??""),
                                    new Claim("TechStaffId",  Convert.ToString(user._StsTechnicalStaff.TechStaffId)??""),
                                    new Claim("StaffName",  Convert.ToString(user._StsTechnicalStaff.StaffName)??""),
                                    new Claim("CustomerName",  Convert.ToString(user._CustomerLogin.CustomerName)??""),
                                    new Claim("CustomerId",  Convert.ToString(user._CustomerLogin.CustomerId)??"")

                                }, "apiauth_type");
               
            }

            return claimsIdentity;
        }

        private string IsUserEmployedBefore1990(GLUser user)
        {
            if (user.HireDate.Value.Year < 1990)
                return "true";
            else
                return "false";
        }
    }
}
