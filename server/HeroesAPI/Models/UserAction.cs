using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeroesAPI.Models
{
    public enum ActionType { VerifyUser = 0 }

    [Table("USER_ACTION")]
    public class UserAction
    {
        [Key]
        [Column("ID")]
        public required int ID { get; set; }
        [Column("USER_ID")]
        public required int UserId { get; set; }
        [Column("ACTION_TYPE")]
        public required ActionType ActionType { get; set; }
        [Column("CREATED_AT")]
        public required DateTime CreatedAt { get; set; }
    }
}
