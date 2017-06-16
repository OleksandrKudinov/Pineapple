using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pineapple.Service.Controllers
{
    public sealed class UsersController : BaseController
    {
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
        // TODO : make restrictions!
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
