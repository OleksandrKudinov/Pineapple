using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected String _connectionString = "";
    }
}
