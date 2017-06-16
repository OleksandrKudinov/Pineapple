using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pineapple.Database;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected Boolean IsMasterKeyValid(String masterkey)
        {
            var mk = config["MasterKey"];
            return mk == masterkey;
        }

        private IConfigurationRoot config => (IConfigurationRoot)this.HttpContext.RequestServices.GetService(typeof(IConfigurationRoot));

        protected PineappleContext RequestDbContext => (PineappleContext)this.HttpContext.RequestServices.GetService(typeof(PineappleContext));
    }
}
