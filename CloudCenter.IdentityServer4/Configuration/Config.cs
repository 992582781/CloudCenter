using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.Configuration
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
           new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
           };

        /// <summary>
        /// 定义作用域
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("CloudCenter.MVC"),//域
             
                new ApiScope("CloudCenter.APi"),//域
            };



        public static IEnumerable<ApiResource> ApiResources =>
           new List<ApiResource>
            {
                new ApiResource("CloudCenter.MVC","CloudCenter.MVC")
                {
                      //4.x必须写
                    Scopes = {new string("CloudCenter.MVC") }//域
                },

                new ApiResource("CloudCenter.APi", "CloudCenter.APi Servic")//ApiName
                {
                     //4.x必须写
                    Scopes = {new string("CloudCenter.APi") }//域
                },
            };


        /// <summary>
        /// 四种Client应用场景。
        /// （1）客户端模式（AllowedGrantTypes = GrantTypes.ClientCredentials）
        ///  这是一种最简单的授权方式，应用于服务于服务之间的通信，
        ///  token通常代表的是客户端的请求，而不是用户。
        ///  使用这种授权类型，会向token endpoint发送token请求，并获得代表客户机的access token。
        ///  客户端通常必须使用token endpoint的Client ID和secret进行身份验证。
        ///  适用场景：用于和用户无关，服务与服务之间直接交互访问资源

        /// （2）密码模式（ClientAllowedGrantTypes = GrantTypes.ResourceOwnerPassword）
        ///  该方式发送用户名和密码到token endpoint，向资源服务器请求令牌。这是一种“非交互式”授权方法。
        ///  官网上称，为了解决一些历史遗留的应用场景，所以保留了这种授权方式，但不建议使用。
        ///  适用场景：用于当前的APP是专门为服务端设计的情况。

        /// （3）混合模式和客户端模式（ClientAllowedGrantTypes =GrantTypes.HybridAndClientCredentials）
        ///  ClientCredentials授权方式在第一种应用场景已经介绍了，这里主要介绍Hybrid授权方式。
        ///  Hybrid是由Implicit和Authorization code结合起来的一种授权方式。其中Implicit用于身份认证，
        ///  ID token被传输到浏览器并在浏览器进行验证；
        ///  而Authorization code使用反向通道检索token和刷新token。
        ///  推荐使用Hybrid模式。
        ///  适用场景：用于MVC框架，服务器端 Web 应用程序和原生桌面/移动应用程序。

        /// （4）简化模式（ClientAllowedGrantTypes =GrantTypes.Implicit）
        ///  Implicit要么仅用于服务端和JavaScript应用程序端进行身份认证，要么用于身份身份验证和access token的传输。
        ///  在Implicit中，所有token都通过浏览器传输的。
        ///  适用场景：JavaScript应用程序。
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                // JavaScript Client
                new Client
                {
                   ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowAccessTokensViaBrowser = true,
                    RequirePkce = false,
                    RequireClientSecret = false,
                    RequireConsent = false,
                    RedirectUris =           { "http://localhost:7001/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:7001/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:7001" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         "CloudCenter.APi"
                    },
                },
                 new Client
                {

                    ClientId = "CloudCenter.MVC",
                    ClientName = "CloudCenter.MVC",
                    //指定允许令牌或授权码返回的地址（URL）
                    RedirectUris={ "http://localhost:5002/signin-oidc", "http://localhost:5005/signin-oidc" },
                     //指定允许注销后返回的地址(URL)，这里写两个客户端
                    PostLogoutRedirectUris={ "http://localhost:5002/signout-callback-oidc", "http://localhost:5005/signout-callback-oidc" },
                    //ClientSecrets = { new Secret("CloudCenter.MVC_Secret".Sha256()) },
                    AllowedScopes = {
                            IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                            "CloudCenter.MVC"
                     },
                    ClientSecrets = { new Secret("CloudCenter.MVC_Secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    RequirePkce = false,//v4.x需要配置这个
                    //AllowedGrantTypes = GrantTypes.Code,
                    AlwaysIncludeUserClaimsInIdToken=true,
                      //允许将token通过浏览器传递
                    AllowAccessTokensViaBrowser=true,
                     //如果不需要显示否同意授权 页面 这里就设置为false
                    RequireConsent=false,
                    AllowOfflineAccess = true,//如果要获取refresh_tokens ,必须把AllowOfflineAccess设置为true
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                 },

                 new Client
                {
                    ClientId = "CloudCenter.APi",
                    ClientName = "CloudCenter.APi Swagger UI",
                    ClientSecrets = { new Secret("CloudCenter".Sha256()) },
                    RedirectUris = { $"http://localhost:5003/swagger/oauth2-redirect.html","http://localhost:5004/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"http://localhost:5003/swagger/",$"http://localhost:5004/swagger/" },
                    AllowedScopes =
                    {
                        "CloudCenter.APi"
                    },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AlwaysIncludeUserClaimsInIdToken=true,
                      //允许将token通过浏览器传递
                    AllowAccessTokensViaBrowser = true,
                     // 是否需要同意授权 （默认是false）
                    RequireConsent=false,
                 },
            };
    }
}
