using HeroesAPI.Helpers;
using HeroesAPI.Models;
using HeroesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowersController(ServiceCosmosDb serviceCosmos, HelperDocumentId helperDocumentId) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Power>>> GetPowers()
        {
            return Ok(await serviceCosmos.GetPowersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Power?>> FindPower(string id)
        {
            var power = await serviceCosmos.FindPowerAsync(id);
            if (power == null)
            {
                return NotFound();
            }

            return Ok(power);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePower(NewPowerForm newPower)
        {
            Power power = new()
            {
                Id = helperDocumentId.GetNewId(),
                Name = newPower.Name,
            };
            await serviceCosmos.InsertPowerAsync(power);

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Power?>> UpdatePower(Power power)
        {
            var updatedPower = await serviceCosmos.UpdatePowerAsync(power);
            return Ok(updatedPower);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePower(string id)
        {
            await serviceCosmos.DeletePowerAsync(id);
            return NoContent();
        }
    }
}
