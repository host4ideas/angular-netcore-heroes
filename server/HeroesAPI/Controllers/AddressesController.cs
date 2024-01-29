using Microsoft.AspNetCore.Mvc;
using HeroesAPI.Models;
using HeroesAPI.Repositories;
using HeroesAPI.Models.Forms;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController(RepositoryAddressesSql repositoryAddresses) : ControllerBase
    {
        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(int id)
        {
            var address = await repositoryAddresses.FindAddressByIdAsync(id);

            if (address == null)
                return NotFound();

            return address;
        }

        // PUT: api/Addresses/5
        [HttpPut]
        public async Task<ActionResult> PutAddress(Address address)
        {
            var updatedAddress = await repositoryAddresses.UpdateAddressAsync(address);

            if (updatedAddress == null)
                return NotFound();

            return NoContent();
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(List<NewAddressForm> addresses)
        {
            var createdAddresses = await repositoryAddresses.InsertAddressesAsync(addresses);
            return StatusCode(StatusCodes.Status201Created, new { addresses = createdAddresses });
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAddress(int id)
        {
            var address = await repositoryAddresses.FindAddressByIdAsync(id);
            if (address == null)
                return NotFound();

            await repositoryAddresses.DeleteAddressAsync(address);

            return NoContent();
        }
    }
}
