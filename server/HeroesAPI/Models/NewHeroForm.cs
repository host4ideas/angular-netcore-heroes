using Newtonsoft.Json;

namespace HeroesAPI.Models
{
    public class NewHeroForm
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }
        public Power Power { get; set; }
        public string? AlterEgo { get; set; }
    }
}
