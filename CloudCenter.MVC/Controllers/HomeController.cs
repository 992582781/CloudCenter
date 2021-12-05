using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CloudCenter.MVC.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudCenter.MVC.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            ViewData["Message"] = "Secure page.";
            return View();
        }

        public async Task<IActionResult> LogoutAsync()
        {
            //首先要清除本地的Cookie，这个Cookie的名字要与之前配置(Startup.cs中AddAuthentication方法)的默认方案里的名字一致，这一步就相当于登出MVC客户端。
            //后一行代码的作用是跳转回到Identity Provider，然后用户可以继续登出IDP, 也就是IDP会清除它的Cookie。
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            var homeUrl = Url.Action(nameof(HomeController.Index), "Home");
            return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme,
                new AuthenticationProperties { RedirectUri = homeUrl });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 测试请求API资源(api1)
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> getApi()
        {
            var client = new HttpClient();
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return Json(new { msg = "accesstoken 获取失败" });
            }

            // client.SetBearerToken(accesstoken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //var httpResponse = await client.GetAsync("http://localhost:5003/api/identity/GetUserClaims");
            //var result = await httpResponse.Content.ReadAsStringAsync();
            //if (!httpResponse.IsSuccessStatusCode)
            //{
            //    return Json(new { msg = "请求 api1 失败。", error = result });
            //}
            //return Json(new
            //{
            //    msg = "成功",
            //    data = JsonConvert.DeserializeObject(result)
            //});
            var content = await client.GetStringAsync("http://localhost:5003/api/identity/GetUserClaims");

            ViewBag.Json = JArray.Parse(content).ToString();
            return View("json");
        }
    }
}
