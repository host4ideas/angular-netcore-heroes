using HeroesAPI.Data;
using HeroesAPI.Models;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;

namespace HeroesAPI.Repositories
{
    public class RepositoryUsersSql(HeroesAppContext heroesAppContext)
    {
        #region USERS

        public bool AppUserExists(int id)
        {
            return heroesAppContext.Users.Any(e => e.Id == id);
        }

        public async Task<AppUser?> FindUserByIdAsync(int id)
        {
            return await heroesAppContext.Users.FindAsync(id);
        }

        public async Task<AppUser?> FindUserByEmailAsync(string email)
        {
            return await heroesAppContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
        }

        public Task<List<AppUser>> GetUsersAsync()
        {
            return heroesAppContext.Users.ToListAsync();
        }

        private async Task<int> GetMaxUserAsync()
        {
            if (!heroesAppContext.Users.Any())
            {
                return 1;
            }

            return await heroesAppContext.Users.MaxAsync(x => x.Id) + 1;
        }

        public async Task<AppUser> InsertUserAsync(string email, string name)
        {
            var response = await heroesAppContext.Users.AddAsync(new()
            {
                Id = await GetMaxUserAsync(),
                Name = name,
                Email = email
            });
            await heroesAppContext.SaveChangesAsync();
            return response.Entity;
        }

        public async Task<AppUser?> UpdateUserAsync(AppUser updatedUser)
        {
            heroesAppContext.Users.Entry(updatedUser).State = EntityState.Modified;

            try
            {
                await heroesAppContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(updatedUser.Id))
                {
                    return default;
                }
                else
                {
                    throw;
                }
            }

            return updatedUser;
        }

        public async Task DeleteUserAsync(AppUser user)
        {
            heroesAppContext.Remove(user);
            await heroesAppContext.SaveChangesAsync();
        }

        #endregion

        #region USER_ADDRESS

        public async Task<UserAddress?> FindUserAddressByIdAsync(int id)
        {
            return await heroesAppContext.UsersAddress.FindAsync(id);
        }

        private async Task<int> GetMaxUserAddressAsync()
        {
            if (!heroesAppContext.UsersAddress.Any())
            {
                return 1;
            }

            return await heroesAppContext.UsersAddress.MaxAsync(x => x.Id) + 1;
        }

        public async Task<List<UserAddress>> InsertUserAddressesAsync(int userId, List<Address> addresses)
        {
            List<UserAddress> userAddresses = [];

            int userAddressIndex = await this.GetMaxUserAddressAsync();

            for (int i = 0; i < addresses.Count; i++)
            {
                userAddressIndex += 1;

                userAddresses.Add(new()
                {
                    Id = userAddressIndex,
                    UserId = userId,
                    AddressId = addresses[i].Id,
                });
            }

            await heroesAppContext.UsersAddress.AddRangeAsync(userAddresses);
            await heroesAppContext.SaveChangesAsync();
            return userAddresses;
        }

        public async Task<UserAddress?> UpdateUserAddressAsync(UserAddress updatedUserAddress)
        {
            UserAddress? userAddress = await FindUserAddressByIdAsync(updatedUserAddress.Id);

            if (userAddress == null)
            {
                return userAddress;
            }

            userAddress.UserId = updatedUserAddress.UserId;
            userAddress.AddressId = updatedUserAddress.AddressId;

            await heroesAppContext.SaveChangesAsync();
            return userAddress;
        }

        public Task DeleteUserAddressAsync(UserAddress userAddress)
        {
            heroesAppContext.UsersAddress.Remove(userAddress);
            return heroesAppContext.SaveChangesAsync();
        }

        #endregion

        #region USER_ACTION

        public async Task<UserAction?> FindUserActionByIdAsync(int id)
        {
            return await heroesAppContext.UserActions.FindAsync(id);
        }

        #endregion
    }
}
