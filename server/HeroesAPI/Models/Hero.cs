using Newtonsoft.Json;

namespace HeroesAPI.Models
{
    public class Hero
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Power Power { get; set; }
        public string? AlterEgo { get; set; }
    }
}
