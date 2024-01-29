using HeroesAPI.Helpers;
using HeroesAPI.Models.Forms;
using HeroesAPI.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using MyApi.Filters;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(RepositoryUsersSql repositoryUsers, RepositoryAddressesSql repositoryAddresses, HelperOAuthToken helperOAuth) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<string>> LoginEmail([FromBody] string email)
        {
            var user = await repositoryUsers.FindUserByEmailAsync(email);

            if (user == null)
            {
                return Unauthorized();
            }

            SigningCredentials credentials = new(helperOAuth.GetKeyToken(), SecurityAlgorithms.HmacSha512);

            string jsonAppUser = JsonConvert.SerializeObject(user);
            var tokenExpires = DateTime.UtcNow.AddMinutes(30);
            Claim[] payload =
            [
                new(ClaimTypes.UserData, jsonAppUser),
                new(ClaimTypes.Role, user.Role.ToString()),
            ];

            JwtSecurityToken token = new(
                claims: payload,
                issuer: helperOAuth.Issuer,
                audience: helperOAuth.Audience,
                signingCredentials: credentials,
                expires: tokenExpires,
                notBefore: DateTime.UtcNow
                );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult> SignUp([FromBody] NewUserForm newUser)
        {
            try
            {
                var user = await repositoryUsers.InsertUserAsync(newUser.Email, newUser.Name);
                var addresses = await repositoryAddresses.InsertAddressesAsync(newUser.NewAddresses);
                await repositoryUsers.InsertUserAddressesAsync(user.Id, addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

            return NoContent();
        }
    }
}
