using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected PineappleContext RequestDbContext => (PineappleContext)this.HttpContext.RequestServices.GetService(typeof(PineappleContext));
    }
}
