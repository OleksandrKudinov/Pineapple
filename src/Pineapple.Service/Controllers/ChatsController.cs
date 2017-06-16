using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database.Models;
using Pineapple.Service.Models;
using Pineapple.Service.Models.Binding;

namespace Pineapple.Service.Controllers
{
    public class ChatsController : BaseController
    {

        [HttpGet]
        [Route("all")]
        // TODO : make restrictions!
        public async Task<IActionResult> GetAllChats()
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    var chats = await context.Chats.Include(x => x.Users).ToArrayAsync();
                    return Ok(chats.Select(chat =>
                    new
                    {
                        chat.ChatId,
                        chat.ChatName,
                        //Users = chat.Users.Select(
                        //    user=>
                        //    new
                        //    {
                        //        user.UserId,
                        //        user.UserName
                        //    })
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats()
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    var principalName = HttpContext.User.Claims.FirstOrDefault().Value;

                    var currentUser = await context.Users.Include(x => x.Chats).FirstOrDefaultAsync(x => x.UserName == principalName);

                    return Ok(currentUser.Chats.Select(chat =>
                    new
                    {
                        chat.ChatId,
                        chat.ChatName,
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] ChatBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var principalName = HttpContext.User.Claims.FirstOrDefault().Value;

                using (var context = RequestDbContext)
                {
                    var chatCreator = await context.Users.FirstOrDefaultAsync(x => x.UserName == principalName);

                    var chat = new Chat()
                    {
                        ChatName = model.ChatName
                    };
                    chat.Users = new List<User>();
                    chat.Users.Add(chatCreator);
                    context.Chats.Add(chat);
                    await context.SaveChangesAsync();
                    return Ok(new
                    {
                        chat.ChatId,
                        chat.ChatName
                    });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("{chatId}/users/{userId}")]
        public async Task<IActionResult> AddUserToChat([FromRoute]Int32 chatId, [FromRoute]Int32 userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var context = RequestDbContext)
                {
                    var userName = HttpContext.User.Claims.FirstOrDefault().Value;

                    var chat = await context.Chats
                        .Include(x => x.Users)
                        .FirstOrDefaultAsync(x => x.ChatId == chatId);

                    if (chat == null)
                    {
                        return BadRequest($"Chat {chatId} does not exist");
                    }

                    if (!chat.Users.Any(x => x.UserName == userName))
                    {
                        return BadRequest($"You are not member of chat {chatId}");
                    }

                    var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

                    if (user == null)
                    {
                        return BadRequest($"User {userId} does not exist");
                    }

                    if (chat.Users.Any(x => x.UserId == user.UserId))
                    {
                        return BadRequest($"User {user.UserId} has been already added into the chat {chat.ChatId}");
                    }

                    chat.Users.Add(user);
                    await context.SaveChangesAsync();
                    return Ok();
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("{chatId}/messages")]
        public async Task<IActionResult> GetMessagesFromChat([FromRoute] Int32 chatId, [FromQuery] Int32 offset = 0, [FromQuery] Int32 count = 10)
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    Message[] messages = await context.Messages
                        .Include(message => message.User)
                        .Include(message => message.Chat)
                        .Where(message => message.Chat.ChatId == chatId)
                        .OrderByDescending(message => message.SendDate)
                        .Skip(offset)
                        .Take(count).
                        ToArrayAsync();

                    return Ok(messages.Select(message => new
                    {
                        message.MessageId,
                        User = new
                        {
                            message.User.UserId,
                            message.User.UserName
                        },
                        message.Text,
                        message.SendDate,
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpPost]
        [Route("{chatId}/messages")]
        public async Task<IActionResult> SendMessageToChat([FromRoute] Int32 chatId, [FromBody] MessageBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var context = RequestDbContext)
                {
                    var principalName = HttpContext.User.Claims.FirstOrDefault().Value;

                    var chat = await context.Chats.Include(x => x.Users).FirstOrDefaultAsync(x => x.ChatId == chatId);

                    if (chat == null)
                    {
                        return BadRequest($"Chat with {chatId} does not exist");
                    }

                    var chatMember = chat.Users.FirstOrDefault(x => x.UserName == principalName);

                    if (chatMember == null)
                    {
                        return BadRequest($"User does not exist in chat {chatId}");
                    }

                    var message = new Message()
                    {
                        User = chatMember,
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

        [HttpGet]
        [Route("{chatId}/users")]
        public async Task<IActionResult> GetUsersFromChat([FromRoute] Int32 chatId)
        {
            try
            {
                using (var context = RequestDbContext)
                {
                    var principalName = HttpContext.User.Claims.FirstOrDefault().Value;

                    Chat chat = await context.Chats
                        .Include(x => x.Users)
                        .FirstOrDefaultAsync(x => x.ChatId == chatId);

                    if (chat == null)
                    {
                        return BadRequest($"Chat {chatId} does not exist");
                    }

                    if (!chat.Users.Any(x => x.UserName == principalName))
                    {
                        return BadRequest($"You are not member of the chat {chatId}");
                    }

                    return Ok(chat.Users.Select(user =>
                        new
                        {
                            user.UserId,
                            user.UserName
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
