using Newtonsoft.Json;

namespace HeroesAPI.Models.Forms
{
    public class NewHeroForm
    {
        [JsonProperty(PropertyName = "id")]
        public string Name { get; set; }
        public string PowerId { get; set; }
        public string? AlterEgo { get; set; }
    }
}
