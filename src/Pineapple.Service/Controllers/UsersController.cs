using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;
using Pineapple.Database.Models;

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
                using (var context = RequestDbContext)
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

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    var users = await context.Users.ToArrayAsync();
                    return Ok(users.Select(user => new { user.UserId, user.UserName}));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("{userId}/chats")]
        public async Task<IActionResult> GetUserChats([FromRoute] Int32 userId)
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    var user = await context.Users.Include(u=>u.Chats).FirstOrDefaultAsync(x=>x.UserId == userId);
                    return Ok(user.Chats.Select(x=>new
                    {
                        x.ChatId,
                        x.ChatName
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
