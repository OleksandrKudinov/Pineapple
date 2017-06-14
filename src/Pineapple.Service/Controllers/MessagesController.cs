using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;
using Pineapple.Database.Models;

namespace Pineapple.Service.Controllers
{
    public sealed class MessagesController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    Message[] messages = await context.Messages.ToArrayAsync();
                    return Ok(messages);
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
                using (var context = new PineappleContext(_connectionString))
                {
                    Message message = await context.Messages.FirstOrDefaultAsync(x=>x.MessageId == id);
                    return Ok(message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] Message message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var context = new PineappleContext(_connectionString))
                {
                    if (!await context.Users.AnyAsync(user => user.UserId == message.UserFromId))
                    {
                        return BadRequest($"{nameof(message.UserFromId)} is invalid");
                    }

                    if (!await context.Users.AnyAsync(user => user.UserId == message.UserToId))
                    {
                        return BadRequest($"{nameof(message.UserToId)} is invalid");
                    }

                    context.Messages.Add(message);
                    await context.SaveChangesAsync();
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
