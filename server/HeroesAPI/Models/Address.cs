using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HeroesAPI.Models
{
    public enum StreetType : byte { Street = 0, Avenue = 1, Square = 2 }

    [Table("ADDRESS")]
    public class Address
    {
        [Key]
        [Column("ID")]
        public required int Id { get; set; }
        [Column("TYPE")]
        public required StreetType Type { get; set; }
        [Column("NUMBER")]
        public int? Number { get; set; }
        [Column("ZIP")]
        public required string Zip { get; set; }
        [Column("STREET")]
        public required string Street { get; set; }
        [Column("COUNTRY")]
        public required string Country { get; set; }
    }
}
