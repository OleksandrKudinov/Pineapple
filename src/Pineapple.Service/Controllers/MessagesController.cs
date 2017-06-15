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

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] MessageBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var context = RequestDbContext)
                {
                    var chat = await context.Chats.Include(x => x.Users).FirstOrDefaultAsync(x => x.ChatId == model.ChatId);

                    if (chat == null)
                    {
                        return BadRequest($"Chat with {model.ChatId} does not exist");
                    }

                    var user = chat.Users.FirstOrDefault(x => x.UserId == model.UserId);

                    if (user == null)
                    {
                        return BadRequest($"User {model.UserId} does not exist in chat {model.ChatId}");
                    }

                    var message = new Message()
                    {
                        User = user,
                        Chat = chat,
                        Text = model.Text,
                        SendDate = DateTime.UtcNow
                    };

                    context.Messages.Add(message);

                    await context.SaveChangesAsync();
                    return Ok(model);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
