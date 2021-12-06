using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCenter.APi.Controller
{
    [Route("api/[Controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/<HomeController1>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "5003", "5003" };
        }

        // GET api/<HomeController1>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<HomeController1>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<HomeController1>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<HomeController1>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
