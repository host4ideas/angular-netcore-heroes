using System.ComponentModel.DataAnnotations;

namespace HeroesAPI.Models.Forms
{
    public class NewUserForm
    {
        public required string Name { get; set; }
        [EmailAddress(ErrorMessage = "Email inválido")]
        public required string Email { get; set; }
        public required List<NewAddressForm> NewAddresses { get; set; }
    }
}
