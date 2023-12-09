using HeroesAPI.Helpers;
using HeroesAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Container = Microsoft.Azure.Cosmos.Container;

namespace HeroesAPI.Services
{
    public class ServiceCosmosDb(CosmosClient client, HelperCosmosDb helperCosmos)
    {
        readonly string database = "heroeswebapp";
        readonly string containerHeroesName = "containerheroes";
        readonly string containerPowersName = "containerpowers";
        private readonly Container containerHeroes = helperCosmos.GetContainerHeroes();
        private readonly Container containerPowers = helperCosmos.GetContainerPowers();

        /// <summary>
        /// Method to create the database and container
        /// </summary>
        public async Task CreateDatabaseAsync()
        {
            ContainerProperties propertiesHeroes = new(this.containerHeroesName, "/id");
            ContainerProperties propertiesPowers = new(this.containerPowersName, "/id");
            await client.CreateDatabaseIfNotExistsAsync(this.database);
            await client.GetDatabase(this.database).CreateContainerIfNotExistsAsync(propertiesHeroes);
            await client.GetDatabase(this.database).CreateContainerIfNotExistsAsync(propertiesPowers);
        }

        #region heroes

        /// <summary>
        /// Get all heroes from the container
        /// </summary>
        public async Task<List<Hero>> GetHeroesAsync()
        {
            List<Hero> results = [];
            // LINQ query generation
            using FeedIterator<Hero> setIterator = containerHeroes.GetItemQueryIterator<Hero>();
            //Asynchronous query execution
            while (setIterator.HasMoreResults)
            {
                results.AddRange(await setIterator.ReadNextAsync());
            }
            return results;
        }

        /// <summary>
        /// Gets a hero by its id
        /// </summary>
        /// <param name="id"></param>
        public async Task<Hero?> FindHeroAsync(string id)
        {
            ItemResponse<Hero> response = await containerHeroes.ReadItemAsync<Hero>(id, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Creates a new hero in the container
        /// </summary>
        /// <param name="hero"></param>
        public async Task InsertHeroAsync(Hero hero)
        {
            await containerHeroes.CreateItemAsync(hero);
        }

        /// <summary>
        /// Updates or inserts a hero form the heroes container and returns it
        /// </summary>
        /// <param name="hero"></param>
        /// <returns>The updated or inserted hero</returns>
        public async Task<Hero> UpdateHeroAsync(Hero hero)
        {
            var response = await containerHeroes.UpsertItemAsync<Hero>(hero, new PartitionKey(hero.Id.ToString()));
            return response.Resource;
        }

        /// <summary>
        /// Deletes a hero from the container
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteHeroAsync(string id)
        {
            await containerHeroes.DeleteItemAsync<Hero>(id, new PartitionKey(id));
        }

        #endregion

        #region powers

        /// <summary>
        /// Retrieves all powers from powers container
        /// </summary>
        /// <returns>Generic list of Powers</returns>
        public async Task<List<Power>> GetPowersAsync()
        {
            List<Power> results = [];
            // LINQ query generation
            using FeedIterator<Power> setIterator = containerPowers.GetItemQueryIterator<Power>();
            //Asynchronous query execution
            while (setIterator.HasMoreResults)
            {
                results.AddRange(await setIterator.ReadNextAsync());
            }
            return results;
        }

        /// <summary>
        /// Finds a power by its id from the powers container
        /// </summary>
        /// <param name="id"></param>
        public async Task<Power?> FindPowerAsync(string id)
        {
            ItemResponse<Power> response = await containerPowers.ReadItemAsync<Power>(id, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Inserts a new power to the powers container
        /// </summary>
        /// <param name="power"></param>
        public async Task InsertPowerAsync(Power power)
        {
            await containerPowers.CreateItemAsync(power);
        }

        /// <summary>
        /// Updates or inserts a new power from the powers container and returns it
        /// </summary>
        /// <param name="power"></param>
        /// <returns>The updated or inserted power</returns>
        public async Task<Power> UpdatePowerAsync(Power power)
        {
            var response = await containerPowers.UpsertItemAsync(power);
            return response.Resource;
        }

        /// <summary>
        /// Deletes a power from the powers container by its id
        /// </summary>
        /// <param name="id"></param>
        public async Task DeletePowerAsync(string id)
        {
            await containerPowers.DeleteItemAsync<Power>(id, new PartitionKey(id));
        }

        #endregion
    }
}
