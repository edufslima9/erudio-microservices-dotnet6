using Duende.IdentityServer.Stores;
using Duende.IdentityServer;
using foo.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using foo.Pages.Login;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using GeekShopping.IdentityServer.Model;
using IdentityModel;
using System.Security.Claims;
using static foo.Pages.Login.ViewModel;

namespace GeekShopping.IdentityServer.Pages.Account.Register
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class Register : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IIdentityProviderStore _identityProviderStore;

        public Register(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IIdentityServerInteractionService interaction, IClientStore clientStore, IEventService events, IAuthenticationSchemeProvider schemeProvider, IIdentityProviderStore identityProviderStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _schemeProvider = schemeProvider;
            _identityProviderStore = identityProviderStore;
        }

        [BindProperty]
        public RegisterViewModel RegisterVM { get; set; }

        public async Task<IActionResult> OnGet(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;

            await BuildRegisterViewModelAsync(returnUrl);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(RegisterVM.ReturnUrl);

            // the user clicked the "cancel" button
            if (RegisterVM.Button != "Register")
            {
                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (context.IsNativeClient())
                    {
                        // The client is native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage(RegisterVM.ReturnUrl);
                    }

                    return Redirect(RegisterVM.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {

                var user = new ApplicationUser
                {
                    UserName = RegisterVM.Username,
                    Email = RegisterVM.Email,
                    EmailConfirmed = true,
                    FirstName = RegisterVM.FirstName,
                    LastName = RegisterVM.LastName
                };

                var result = await _userManager.CreateAsync(user, RegisterVM.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(RegisterVM.RoleName).GetAwaiter().GetResult())
                    {
                        var userRole = new IdentityRole
                        {
                            Name = RegisterVM.RoleName,
                            NormalizedName = RegisterVM.RoleName,

                        };
                        await _roleManager.CreateAsync(userRole);
                    }

                    await _userManager.AddToRoleAsync(user, RegisterVM.RoleName);

                    await _userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim(JwtClaimTypes.Name, RegisterVM.Username),
                    new Claim(JwtClaimTypes.Email, RegisterVM.Email),
                    new Claim(JwtClaimTypes.FamilyName, RegisterVM.FirstName),
                    new Claim(JwtClaimTypes.GivenName, RegisterVM.LastName),
                    new Claim(JwtClaimTypes.WebSite, $"http://{RegisterVM.Username}.com"),
                    new Claim(JwtClaimTypes.Role,"User") });

                    var loginresult = await _signInManager.PasswordSignInAsync(RegisterVM.Username, RegisterVM.Password, false, lockoutOnFailure: true);
                    if (loginresult.Succeeded)
                    {
                        var checkuser = await _userManager.FindByNameAsync(RegisterVM.Username);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(checkuser.UserName, checkuser.Id, checkuser.UserName, clientId: context?.Client.ClientId));

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {
                                // The client is native, so this change in how to
                                // return the response is for better UX for the end user.
                                return this.LoadingPage(RegisterVM.ReturnUrl );
                            }

                            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                            return Redirect(RegisterVM.ReturnUrl);
                        }

                        // request for a local page
                        if (Url.IsLocalUrl(RegisterVM.ReturnUrl))
                        {
                            return Redirect(RegisterVM.ReturnUrl);
                        }
                        else if (string.IsNullOrEmpty(RegisterVM.ReturnUrl))
                        {
                            return Redirect("~/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new Exception("invalid return URL");
                        }
                    }

                }
            }

            // something went wrong, show form with error
            await BuildRegisterViewModelAsync(RegisterVM.ReturnUrl);
            return Page();
        }

        private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            List<string> roles = new List<string>();
            roles.Add("Admin");
            roles.Add("Client");
            ViewData["message"] = roles;
            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new RegisterViewModel
                {
                    EnableLocalLogin = local,
                    ReturnUrl = returnUrl,
                    Username = context?.LoginHint,
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName ?? x.Name,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.Client.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new RegisterViewModel
            {
                AllowRememberLogin = LoginOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }
    }
}
