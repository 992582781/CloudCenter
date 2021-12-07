using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCenter.Infrastructure
{
    public class IContext
    {
        public static IHttpContextAccessor Accessor;
        public static HttpContext GetContext()
        {
            return Accessor.HttpContext;
        }
    }
}
