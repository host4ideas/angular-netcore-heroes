using HeroesAPI.Helpers;
using HeroesAPI.Models;
using HeroesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController(ServiceCosmosDb serviceCosmosDb, HelperDocumentId helperDocumentId) : ControllerBase
    {
        //[HttpPost("[action]")]
        //public async Task<ActionResult> CreateDatabase()
        //{
        //    await serviceCosmosDb.CreateDatabaseAsync();
        //    return NoContent();
        //}

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
        public async Task<ActionResult> CreateHero(NewHeroForm hero)
        {
            Hero newHero = new()
            {
                Id = helperDocumentId.GetNewId(),
                Name = hero.Name,
                Power = hero.Power,
                AlterEgo = hero.AlterEgo,
            };
            await serviceCosmosDb.InsertHeroAsync(newHero);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Hero?>> UpdateHero(Hero hero)
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
