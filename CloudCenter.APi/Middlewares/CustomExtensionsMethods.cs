using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using CloudCenter.APi.Middlewares;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CloudCenter.APi.Middlewares
{
    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CloudCenter.APi HTTP API",//文档标题
                    Version = "v1",//版本号
                    Description = "The CloudCenter.APi Service HTTP API"//文档描述
                });
                //添加Swagger锁
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置（请求头）
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"http://localhost:5001/connect/authorize"),
                            TokenUrl = new Uri($"http://localhost:5001/connect/token"),
                            Scopes = new Dictionary<string, string>()
                        {
                            { "CloudCenter.APi", "CloudCenter.APi API" }
                        }
                        }
                    }
                });
                //Determine base path for the application.  
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "CloudCenter.APi.xml");
                options.IncludeXmlComments(xmlPath);
                //在header中添加token，传递到后台
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            //关闭了 JWT 身份信息类型映射
            //这样就允许 well-known 身份信息（比如，“sub” 和 “idp”）无干扰地流过。
            //这个身份信息类型映射的“清理”必须在调用 AddAuthentication()之前完成
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            var identityUrl = "http://localhost:5001";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = identityUrl;
                options.RequireHttpsMetadata = false;//是否启用https
                options.Audience = "CloudCenter.APi";//资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
            });

            #region ① 用ids4包 

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddIdentityServerAuthentication(JwtBearerDefaults.AuthenticationScheme, options =>
            //  {
            //      options.Authority = "http://localhost:5001";
            //      options.RequireHttpsMetadata = false;
            //      options.ApiName = "api1";
            //      options.ApiSecret = "apipwd"; //对应ApiResources中的密钥
            //  });
            //services.AddAuthorization();
            #endregion

            #region ② 用自带的JwtBearer程序

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            //   {
            //       // IdentityServer 地址
            //       options.Authority = "http://localhost:5001";
            //       //不需要https
            //       options.RequireHttpsMetadata = false;
            //       //这里要和 IdentityServer 定义的 api1 保持一致
            //       options.Audience = "api1";
            //   });

            #endregion

            #region ③ 用自带的JwtBearer程序，并添加授权策略作用域

            //services.AddAuthentication("Bearer")
            //   .AddJwtBearer("Bearer", options =>
            //   {
            //       options.Authority = "http://localhost:5001";
            //       options.RequireHttpsMetadata = false;
            //       options.TokenValidationParameters = new TokenValidationParameters
            //       {
            //           ValidateAudience = false
            //       };
            //   });

            //// adds an authorization policy to make sure the token is for scope 'api1'
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiScope", policy =>
            //    {
            //        policy.RequireAuthenticatedUser();
            //        policy.RequireClaim("scope", "api1");
            //    });
            //});

            #endregion

            return services;
        }
    }
}
