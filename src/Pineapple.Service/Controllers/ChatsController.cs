using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;
using Pineapple.Database.Models;

namespace Pineapple.Service.Controllers
{
    public class ChatsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetChats()
        {
            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    var chats = await context.Chats.Include(x=>x.Users).ToArrayAsync();
                    return Ok(chats.Select(chat=>
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

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(Int32 chatId)
        {
            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    var chats = await context.Chats
                        .Include(x => x.Users)
                        .Include(x=>x.Messages)
                        .ToArrayAsync();

                    return Ok(chats.Select(chat =>
                    new
                    {
                        chat.ChatId,
                        chat.ChatName,
                        //Users = chat.Users.Select(
                        //    user =>
                        //    new
                        //    {
                        //        user.UserId,
                        //        user.UserName
                        //    }),
                        //Messages = chat.Messages.Select(
                        //    message=>new
                        //    {
                        //        User = new
                        //        {
                        //            message.User.UserId,
                        //            message.User.UserName
                        //        },
                        //        message.MessageId,
                        //        message.Text,
                        //        message.SendDate,
                        //    })
                    }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateChat([FromBody] Chat model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    context.Chats.Add(model);
                    await context.SaveChangesAsync();
                    return Ok(model);
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
                using (var context = new PineappleContext(_connectionString))
                {
                    var chat = await context.Chats.FirstOrDefaultAsync(x => x.ChatId == chatId);

                    if (chat == null)
                    {
                        return BadRequest($"chat {chatId} does not exist");
                    }

                    var user = await context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

                    if (user == null)
                    {
                        return BadRequest($"user {userId} does not exist");
                    }

                    chat.Users = chat.Users ?? new List<User>();
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
                using (var context = new PineappleContext(_connectionString))
                {
                    Message[] messages = await context.Messages
                        .Include(message => message.User)
                        .Include(message => message.Chat)
                        .Where(message => message.Chat.ChatId == chatId)
                        .OrderByDescending(message=>message.SendDate)
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

        [HttpGet]
        [Route("{chatId}/users")]
        public async Task<IActionResult> GetUsersFromChat([FromRoute] Int32 chatId)
        {
            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    Chat chat = await context.Chats
                        .Include(x => x.Users)
                        .FirstOrDefaultAsync(x=>x.ChatId == chatId);

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
