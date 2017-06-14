using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;

namespace Pineapple.Service.Controllers
{
    public sealed class UsersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                    return Ok(user);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
