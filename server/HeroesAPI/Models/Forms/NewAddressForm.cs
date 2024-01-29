namespace HeroesAPI.Models.Forms
{
    public class NewAddressForm
    {
        public required StreetType Type { get; set; }
        public int? Number { get; set; }
        public required string Zip { get; set; }
        public required string Street { get; set; }
        public required string Country { get; set; }
    }
}
