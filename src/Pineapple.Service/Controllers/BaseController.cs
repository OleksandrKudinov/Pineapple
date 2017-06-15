using System;
using Microsoft.AspNetCore.Mvc;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    public abstract class BaseController : Controller
    {
        protected String _connectionString = @"Data Source=DESKTOP-K21PL9C\MSSQLSERVER2016;Initial Catalog=Pineapple_dev;Integrated Security=True";
    }
}
