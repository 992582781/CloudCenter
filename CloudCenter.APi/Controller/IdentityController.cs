using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.APi.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class IdentityController:ControllerBase
    {

        /// <summary>
        /// 这是一个啥都没干的接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("getUserClaims")]
        [Authorize]
        //[Authorize(Roles ="admin")]
        public IActionResult GetUserClaims()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
