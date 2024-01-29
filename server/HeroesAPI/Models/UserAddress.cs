using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesAPI.Models
{
    [Table("USER_ADDRESS")]
    public class UserAddress
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("USER_ID")]
        public int UserId { get; set; }
        [Column("ADDRESS_ID")]
        public int AddressId { get; set; }
    }
}
