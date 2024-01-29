using HeroesAPI.Helpers;
using HeroesAPI.Models;
using HeroesAPI.Models.Forms;
using HeroesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MyApi.Filters;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController(ServiceCosmosDb serviceCosmosDb, HelperDocumentId helperDocumentId) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Hero>>> GetAllHeroes()
        {
            return Ok(await serviceCosmosDb.GetHeroesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> FindHero(string id)
        {
            var hero = await serviceCosmosDb.FindHeroAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult> CreateHero([FromBody] NewHeroForm hero)
        {
            Power? heroPower = await serviceCosmosDb.FindPowerAsync(hero.PowerId);

            if (heroPower == null)
            {
                return NotFound("Power not found.");
            }

            Hero newHero = new()
            {
                Id = helperDocumentId.GetNewId(),
                Name = hero.Name,
                Power = heroPower,
                AlterEgo = hero.AlterEgo,
            };
            await serviceCosmosDb.InsertHeroAsync(newHero);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Hero?>> UpdateHero([FromBody] Hero hero)
        {
            Hero? updatedHero = await serviceCosmosDb.UpdateHeroAsync(hero);
            return Ok(updatedHero);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHero(string id)
        {
            await serviceCosmosDb.DeleteHeroAsync(id);
            return NoContent();
        }
    }
}
