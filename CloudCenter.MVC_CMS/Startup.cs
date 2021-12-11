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
            var identityUrl = "http://localhost:5001";//IdentityServer4��ַ
            var callBackUrl = "http://localhost:5005";//��վ��ַ
            #region MVC client
            //�ر��� JWT �����Ϣ����ӳ��
            //���������� well-known �����Ϣ�����磬��sub�� �� ��idp�����޸��ŵ�������
            //��������Ϣ����ӳ��ġ����������ڵ��� AddAuthentication()֮ǰ���
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                //DefaultScheme = "Cookies"�����"Cookies"�ַ����ǿ���������д�ģ�ֻҪ���ߵ�һ�¼��ɡ�
                //�������ͬһ�����������кܶ�Ӧ�õĻ������Scheme�����ֲ����ظ���
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //DefaultChanllangeScheme��Ϊ"oidc", ���������������OpenIdConnect������Ҫһ��.
                //���û���Ҫ��½��ʱ��, ��ʹ�õ���OpenId Connect Scheme
                //options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
              //AddCookie�������֮ǰ���õ�DefaultScheme���ƣ���������Cookie�Ĵ����ߣ�
              //����Ӧ�ó���Ϊ���ǵ�DefaultScheme�����˻���Cookie�������֤
              .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
              {
                  //��Ȩ�ޣ���ʾ��ҳ��
                  options.AccessDeniedPath = "/Authorization/AccessDenied";
                  options.ExpireTimeSpan = TimeSpan.FromHours(2);
              })
              //  AddOpenIdConnect��������˶�OpenID Connect���̵�֧�֣�
              //  ��������������ִ��OpenId Connect Э��Ĵ����ߡ���������߻Ḻ�𴴽������֤����
              //  Token������������󣬲�����ID Token����֤���������������֤scheme����֮ǰ���õ�"oidc"��
              //  ������˼��������ÿͻ��˵�ĳ����Ҫ�������֤��ʱ��
              //  OpenID Connect������ΪĬ�Ϸ���������(��Ϊ֮ǰ���õ�DefaultChallengeScheme��"oidc", �����������һ��)��
              .AddOpenIdConnect("oidc", options =>
              {
                  //SignInScheme�������DefaultSchemeһ�£�
                  //����֤�����֤�ɹ��Ľ�����ᱻ�����ڷ�����Ϊ"Cookies"��Cookie�
                  //Authority����Identity Provider�ĵ�ַ��
                  //ClientId��SecretҪ��IdentityProvider�����ֵһ����
                  //�����Scope��openid��profile����ʵ�м��Ĭ��Ҳ��������Щscope������д��������ȷһЩ��
                  //SaveTokens = true����ʾ����洢��Identity Provider�����õ�tokens��
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
                  //Ϊapi��ʹ��refresh_token��ʱ��,����offline_access������
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.RequireHttpsMetadata = false;
                  options.Scope.Add("CloudCenter.MVC"); //�����Ȩ��Դ

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
