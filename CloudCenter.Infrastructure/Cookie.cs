using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCenter.Infrastructure
{
    public class Cookie : ICookie
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Cookie(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        public void SetCookies(string key, string value, int minutes = 300)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        public void DeleteCookies(string key)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        public string GetCookies(string key)
        {
            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }
    }
}
