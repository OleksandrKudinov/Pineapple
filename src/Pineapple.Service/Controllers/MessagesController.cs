using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database.Models;
using Pineapple.Service.Models.Binding;

namespace Pineapple.Service.Controllers
{
    public sealed class MessagesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    Message[] messages = await context.Messages
                        .Include(x=>x.User)
                        .Include(x=>x.Chat)
                        .ToArrayAsync();

                    return Ok(messages.Select(x =>
                    new
                    {
                        x.MessageId,
                        User = new { x.User.UserId, x.User.UserName },
                        Chat = new { x.Chat.ChatId, x.Chat.ChatName },
                        x.Text,
                        x.SendDate
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(Int32 id)
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    Message message = await context.Messages.FirstOrDefaultAsync(x => x.MessageId == id);
                    return Ok(message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
