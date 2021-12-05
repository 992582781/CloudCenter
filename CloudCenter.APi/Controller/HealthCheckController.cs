using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.APi.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
