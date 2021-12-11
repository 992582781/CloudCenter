using CloudCenter.Aop;
using CloudCenter.IdentityServer4.CommandAndQueries;
using CloudCenter.IdentityServer4.Model;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
           ILogger<AccountController> logger,
           IIdentityServerInteractionService interaction,
           IMediator mediator)
        {
            _logger = logger;
            _interaction = interaction;
            _mediator = mediator;
        }


        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet]
        public IActionResult LoginOk(string returnUrl = null)
        {
            return View();
        }

        //内部跳转
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                //如果是本地
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(AccountController.Login), "Login");
        }
        /// <summary>
        /// 登录post回发处理
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            var model = new LoginInputModel();
            model.SubjectId = DateTime.Now.Minute.ToString();
            model.Username = DateTime.Now.Minute.ToString();
            model.Password = "123456";
            model.ReturnUrl = returnUrl;

            _logger.LogError("mediator开始");
            var userQuery = new UserQuery();
            userQuery.Username = DateTime.Now.Minute.ToString();
            userQuery.Password = "i3yuan";
            var tt= await _mediator.Send(userQuery);
            _logger.LogError("mediator结束");

            AuthenticationProperties props = null;
            props = new AuthenticationProperties
            {
                AllowRefresh = true,
                RedirectUri = model.ReturnUrl,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
            };

            var Claims = new List<Claim> {
                            new Claim(JwtClaimTypes.Name, "i3yuan99 Smith99"),
                            new Claim(JwtClaimTypes.GivenName, "i3yuan99"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith99"),
                            new Claim(JwtClaimTypes.Id, "opiiopp"),
                            new Claim(JwtClaimTypes.WebSite, "http://i3yuan.top99"),
                            new Claim("UserID", "369"),
                            //new Claim(JwtClaimTypes.Role, "admin")  //添加角色
                        };

            var isuser = new IdentityServerUser(model.SubjectId)
            {
                DisplayName = model.Username,
                AdditionalClaims = Claims
            };

            await HttpContext.SignInAsync(isuser, props);

            if (_interaction.IsValidReturnUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return Redirect("~/");
        }


        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == true)
            {
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };
            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    //model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }
                string url = "/Account/Logout?logoutId=" + model.LogoutId;
                try
                {

                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }
            // delete authentication cookie
            await HttpContext.SignOutAsync();
            //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);
            return Redirect(logout?.PostLogoutRedirectUri);
        }
    }

    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
    }
}
