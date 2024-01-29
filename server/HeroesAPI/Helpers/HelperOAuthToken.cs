using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HeroesAPI.Helpers
{
    public class HelperOAuthToken(IConfiguration configuration)
    {
        public readonly string Issuer = configuration.GetValue<string>("ApiOAuth:Issuer");
        public readonly string Audience = configuration.GetValue<string>("ApiOAuth:Audience");
        public readonly string SecretKey = configuration.GetValue<string>("ApiOAuth:SecretKey");

        /// <summary>
        /// Generate token from SecretKey
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        /// <summary>
        /// Activates auth services in Program.cs
        /// </summary>
        /// <returns></returns>
        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> options = new(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = this.Issuer,
                        ValidAudience = this.Audience,
                        IssuerSigningKey = this.GetKeyToken()
                    };
            });
            return options;
        }

        /// <summary>
        /// Auth schema
        /// </summary>
        /// <returns></returns>
        public Action<AuthenticationOptions> GetAuthenticationOptions()
        {
            Action<AuthenticationOptions> options = new(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}
