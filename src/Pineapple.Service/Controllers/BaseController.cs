using System;
using Microsoft.AspNetCore.Mvc;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        protected String _connectionString = "";
    }
}
