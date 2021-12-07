using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCenter.Infrastructure
{
    public interface ICookie
    {
        void SetCookies(string key, string value, int minutes = 300);

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        void DeleteCookies(string key);

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        string GetCookies(string key);

    }
}
