using System;
using Microsoft.IdentityModel.Tokens;

namespace Pineapple.Service.Infrastructure.Authorization
{
    public sealed class TokenProviderOptions
    {
        public String Path { get; set; }

        public String Issuer { get; set; }

        public String Audience { get; set; }

        public TimeSpan Expiration { get; set; }

        public SigningCredentials SigningCredentials { get; set; }
    }
}
