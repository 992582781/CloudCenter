using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.CommandAndQueries
{
    /// <summary>
    /// bool是该Query处理后的返回结果的类型
    /// </summary>
    public record UserQuery : IQuery<bool>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }

    public record UserIdQuery : IQuery<string>
    {
        [Required]
        public string UserId { get; set; }
    }
}
