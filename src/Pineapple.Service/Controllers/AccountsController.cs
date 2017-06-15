using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Database.Models;
using Pineapple.Service.Models.Binding;

namespace Pineapple.Service.Controllers
{
    public sealed class AccountsController : BaseController
    {
        [HttpPost]
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
    }
}
