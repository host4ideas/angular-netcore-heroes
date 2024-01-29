using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesAPI.Models
{
    public enum UserRole : byte { User = 0, Admin = 1 }

    [Table("APP_USER")]
    public class AppUser
    {
        [Key]
        [Column("ID")]
        public required int Id { get; set; }
        [Column("NAME")]
        public required string Name { get; set; }
        [Column("EMAIL")]
        public required string Email { get; set; }
        [Column("ROLE")]
        public UserRole Role { get; set; }
    }
}
