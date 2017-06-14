using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database;
using Pineapple.Database.Models;

namespace Pineapple.Service.Controllers
{
    [Route("api/[controller]")]
    public sealed class MessagesController : Controller
    {
        private String _connectionString;

        public MessagesController()
        {
            _connectionString = "";
        }
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
