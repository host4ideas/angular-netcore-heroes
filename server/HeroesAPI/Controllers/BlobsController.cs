using Microsoft.AspNetCore.Mvc;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        // GET: api/Blobs
        [HttpGet]
        public IEnumerable<string> Get()
        {
            // Logic to retrieve a list of blobs
            return new string[] { "blob1", "blob2", "blob3" };
        }

        // POST: api/Blobs
        [HttpPost]
        public void Post([FromBody] string value)
        {
            // Logic to upload a blob
        }

        // GET: api/Blobs/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            // Logic to retrieve a specific blob by ID
            return "blob";
        }

        // DELETE: api/Blobs/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // Logic to delete a blob by ID
        }
    }
}
