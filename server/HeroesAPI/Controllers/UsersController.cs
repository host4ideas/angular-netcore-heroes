using Microsoft.AspNetCore.Mvc;
using HeroesAPI.Models;
using HeroesAPI.Repositories;
using HeroesAPI.Models.Forms;
using Microsoft.AspNetCore.Authorization;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(RepositoryUsersSql repositoryUsers) : ControllerBase
    {
        // GET: api/AppUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await repositoryUsers.GetUsersAsync();
        }

        // GET: api/AppUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUser(int id)
        {
            var appUser = await repositoryUsers.FindUserByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return appUser;
        }

        // PUT: api/AppUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<ActionResult> PutAppUser([FromBody] AppUser appUser)
        {
            try
            {
                if (await repositoryUsers.UpdateUserAsync(appUser) == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return NoContent();
        }

        // POST: api/AppUsers
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostAppUser([FromBody] NewUserForm newUser)
        {
            var user = await repositoryUsers.InsertUserAsync(newUser.Email, newUser.Name);
            return CreatedAtAction("GetAppUser", new { id = user.Id }, user);
        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "ADMIN_ONLY")]
        public async Task<ActionResult> DeleteAppUser(int id)
        {
            var appUser = await repositoryUsers.FindUserByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }

            await repositoryUsers.DeleteUserAsync(appUser);
            return NoContent();
        }
    }
}
