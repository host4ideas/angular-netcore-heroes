using HeroesAPI.Data;
using HeroesAPI.Models;
using HeroesAPI.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace HeroesAPI.Repositories
{
    public class RepositoryAddressesSql(HeroesAppContext heroesAppContext)
    {
        #region ADDRESS

        public async Task<Address?> FindAddressByIdAsync(int id)
        {
            return await heroesAppContext.Address.FindAsync(id);
        }

        private Task<bool> AddressExists(int id)
        {
            return heroesAppContext.Address.AnyAsync(e => e.Id == id);
        }

        private async Task<int> GetMaxAddressAsync()
        {
            if (!heroesAppContext.Address.Any())
            {
                return 1;
            }

            return await heroesAppContext.Address.MaxAsync(x => x.Id) + 1;
        }

        public async Task<List<Address>> InsertAddressesAsync(List<NewAddressForm> addresses)
        {
            List<Address> newAddresses = [];
            int addressIndex = await this.GetMaxAddressAsync();

            for (int i = 0; i < addresses.Count; i++)
            {
                addressIndex += 1;
                newAddresses.Add(new()
                {
                    Id = addressIndex,
                    Street = addresses[i].Street,
                    Country = addresses[i].Country,
                    Number = addresses[i].Number,
                    Zip = addresses[i].Zip,
                    Type = addresses[i].Type
                });
            }

            await heroesAppContext.Address.AddRangeAsync(newAddresses);
            await heroesAppContext.SaveChangesAsync();
            return newAddresses;
        }

        public async Task<Address?> UpdateAddressAsync(Address updatedAddress)
        {
            try
            {
                heroesAppContext.Address.Entry(updatedAddress).State = EntityState.Modified;
                await heroesAppContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AddressExists(updatedAddress.Id))
                {
                    return default;
                }
                else
                {
                    throw;
                }
            }

            return updatedAddress;
        }

        public Task DeleteAddressAsync(Address address)
        {
            heroesAppContext.Address.Remove(address);
            return heroesAppContext.SaveChangesAsync();
        }

        #endregion
    }
}
