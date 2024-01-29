using HeroesAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HeroesAPI.Data
{
    public class HeroesAppContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserAddress> UsersAddress { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<UserAction> UserActions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var roleConverter = new EnumToNumberConverter<UserRole, byte>();
            var addressConverter = new EnumToNumberConverter<StreetType, byte>();

            modelBuilder.Entity<AppUser>()
                .Property(u => u.Role)
                .HasConversion(roleConverter);

            modelBuilder.Entity<Address>()
                .Property(u => u.Type)
                .HasConversion(addressConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
