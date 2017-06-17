using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database.Models;
using Pineapple.Service.Models.Binding;

namespace Pineapple.Service.Controllers
{
    public sealed class AccountsController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAccount([FromBody] AccountRegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var context = RequestDbContext)
                {
                    var account = new Account()
                    {
                        Login = model.Username,
                        PasswordHash = model.Password,
                        User = new User()
                        {
                            UserName = model.Username
                        }
                    };

                    context.Accounts.Add(account);
                    await context.SaveChangesAsync();

                    return Ok(new
                    {
                        account.User.UserName,
                        account.PasswordHash
                    });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{masterkey}")]
        public async Task<IActionResult> GetAccounts(String masterkey)
        {
            if (!IsMasterKeyValid(masterkey))
            {
                return Ok(new int[0]);
            }

            try
            {
                using (var context = RequestDbContext)
                {
                    Account[] accounts = await context.Accounts.ToArrayAsync();
                    return Ok(accounts.Select(x =>
                        new
                        {
                            x.Login,
                            x.PasswordHash
                        }));
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet]
        [Route("whoami")]
        public async Task<IActionResult> GetUserByAuth()
        {
            try
            {
                var principalName = HttpContext.User.Claims.FirstOrDefault().Value;
                using (var context = RequestDbContext)
                {
                    var account =
                        await context.Accounts
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(x => x.User.UserName == principalName);

                    return Ok(new
                    {
                        account.User.UserId,
                        account.User.UserName
                    });
                }
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
