using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pineapple.Service.Infrastructure.Authorization;
using Pineapple.Service.Models.Binding;

namespace Pineapple.Service.Controllers
{
    [AllowAnonymous]
    [Route("auth")]
    public sealed class AuthController : BaseController
    {
        public AuthController(TokenProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        [HttpPost]
        [Route("token")]
        public IActionResult Auth([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            String username = model.Username;
            String password = model.Password;

            ClaimsIdentity identity = username == "user" && password == "password"
                ? new ClaimsIdentity(new System.Security.Principal.GenericIdentity(username, "Token"), new Claim[] { })
                : null;

            if (identity == null)
            {
                return BadRequest("Invalid username or password");
            }

            var now = DateTime.UtcNow;

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            DateTimeOffset offset = new DateTimeOffset(now);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, offset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds
            };
            return Ok(response);
        }

        private readonly TokenProviderOptions _options;
    }
}
