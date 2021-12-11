using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using CloudCenter.MVC_CMS.Extendsions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CloudCenter.MVC_CMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var identityUrl = "http://localhost:5001";//IdentityServer4地址
            var callBackUrl = "http://localhost:5005";//本站地址
            #region MVC client
            //关闭了 JWT 身份信息类型映射
            //这样就允许 well-known 身份信息（比如，“sub” 和 “idp”）无干扰地流过。
            //这个身份信息类型映射的“清理”必须在调用 AddAuthentication()之前完成
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                //DefaultScheme = "Cookies"，这个"Cookies"字符串是可以任意填写的，只要与后边的一致即可。
                //但是如果同一个服务器上有很多应用的话，这个Scheme的名字不能重复。
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //DefaultChanllangeScheme设为"oidc", 这个名字与后边配置OpenIdConnect的名字要一样.
                //当用户需要登陆的时候, 将使用的是OpenId Connect Scheme
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
              //AddCookie其参数是之前配置的DefaultScheme名称，这配置了Cookie的处理者，
              //并让应用程序为我们的DefaultScheme启用了基于Cookie的身份认证
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
              {
                  //无权限，显示的页面
                  options.AccessDeniedPath = "/Authorization/AccessDenied";
                  options.ExpireTimeSpan = TimeSpan.FromHours(2);
              })
              //  AddOpenIdConnect方法添加了对OpenID Connect流程的支持，
              //  它让配置了用来执行OpenId Connect 协议的处理者。这个处理者会负责创建身份认证请求，
              //  Token请求和其它请求，并负责ID Token的验证工作。它的身份认证scheme就是之前配置的"oidc"，
              //  它的意思就是如果该客户端的某部分要求身份认证的时候，
              //  OpenID Connect将会作为默认方案被触发(因为之前设置的DefaultChallengeScheme是"oidc", 和这里的名字一样)。
              .AddOpenIdConnect("oidc", options =>
              {
                  //SignInScheme和上面的DefaultScheme一致，
                  //它保证身份认证成功的结果将会被保存在方案名为"Cookies"的Cookie里。
                  //Authority就是Identity Provider的地址。
                  //ClientId和Secret要与IdentityProvider里面的值一样。
                  //请求的Scope有openid和profile，其实中间件默认也包括了这些scope，但是写出来更明确一些。
                  //SaveTokens = true，表示允许存储从Identity Provider那里获得的tokens。
                  options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                  options.Authority = identityUrl.ToString();
                  options.SignedOutRedirectUri = callBackUrl.ToString();
                  options.ClientId = "CloudCenter.MVC";
                  options.ClientSecret = "CloudCenter.MVC_Secret";
                  options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                  options.Scope.Clear();
                  options.Scope.Add("openid");
                  options.Scope.Add("profile");
                  options.SaveTokens = true;
                  //为api在使用refresh_token的时候,配置offline_access作用域
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.RequireHttpsMetadata = false;
                  options.Scope.Add("CloudCenter.MVC"); //添加授权资源

              });
            #endregion
            services.ConfigureNonBreakingSameSiteCookies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
