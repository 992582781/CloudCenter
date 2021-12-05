using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.IdentityServer4.CommandAndQueries
{
    /// <summary>
    /// 新增 User
    /// </summary>
    /// <remarks>
    /// string 是该处理后的返回结果的类型
    /// </remarks>
    public record CreateUser : ICommand<string>
    {
        /// <summary>
        /// UserId
        /// </summary>
        /// <remarks>This is the customers internal ID of the order.</remarks>      
        /// <example>123</example> 
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public string UserNmae { get; set; }
    }


    /// <summary>
    /// 删除 User
    /// </summary>
    /// <remarks>
    /// </remarks>
    public record RemoveUser : ICommand<string>
    {
        /// <summary>
        /// UserId
        /// </summary>
        /// <remarks>This is the customers internal ID of the order.</remarks>      
        /// <example>123</example> 
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public string UserNmae { get; set; }
    }


    /// <summary>
    /// 修改 User
    /// </summary>
    /// <remarks>
    /// </remarks>
    public record ModifyUser : ICommand<string>
    {
        /// <summary>
        /// UserId
        /// </summary>
        /// <remarks>This is the customers internal ID of the order.</remarks>      
        /// <example>123</example> 
        [Required]
        public string PassWord { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public string UserNmae { get; set; }
    }
}