using HeroesAPI.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Container = Microsoft.Azure.Cosmos.Container;

namespace HeroesAPI.Services
{
    public class ServiceCosmosDb
    {
        private string CosmosDatabaseName { get; set; }
        private string ContainerHeroesName { get; set; }
        private string ContainerPowersName { get; set; }
        private string ContainerUsersName { get; set; }

        private Container ContainerHeroes { get; set; }
        private Container ContainerPowers { get; set; }
        private Container ContainerUsers { get; set; }

        private CosmosClient CosmosClient { get; set; }

        public ServiceCosmosDb(CosmosClient client, IConfiguration configuration)
        {
            this.CosmosClient = client;

            this.CosmosDatabaseName = configuration.GetValue<string>("HeroesCosmosDB:Database");
            this.ContainerHeroesName = configuration.GetValue<string>("HeroesCosmosDB:ContainerHeroes");
            this.ContainerPowersName = configuration.GetValue<string>("HeroesCosmosDB:ContainerPowers");
            this.ContainerUsersName = configuration.GetValue<string>("HeroesCosmosDB:ContainerUsers");

            this.ContainerHeroes = client.GetContainer(CosmosDatabaseName, ContainerHeroesName);
            this.ContainerPowers = client.GetContainer(CosmosDatabaseName, ContainerPowersName);
            this.ContainerUsers = client.GetContainer(CosmosDatabaseName, ContainerUsersName);
        }

        /// <summary>
        /// Creates the database and containers
        /// </summary>
        public async Task CreateDatabaseAsync()
        {
            ContainerProperties propertiesHeroes = new(this.ContainerHeroesName, "/id");
            ContainerProperties propertiesPowers = new(this.ContainerPowersName, "/id");
            ContainerProperties propertiesUsers = new(this.ContainerUsersName, "/id");
            await this.CosmosClient.CreateDatabaseIfNotExistsAsync(this.CosmosDatabaseName);
            await this.CosmosClient.GetDatabase(this.CosmosDatabaseName).CreateContainerIfNotExistsAsync(propertiesHeroes);
            await this.CosmosClient.GetDatabase(this.CosmosDatabaseName).CreateContainerIfNotExistsAsync(propertiesPowers);
            await this.CosmosClient.GetDatabase(this.CosmosDatabaseName).CreateContainerIfNotExistsAsync(propertiesUsers);
        }

        #region USERS

        /// <summary>
        /// Gets a user by its id
        /// </summary>
        /// <param name="id"></param>
        public async Task<AppUser?> FindUserByIdAsync(string id)
        {
            ItemResponse<AppUser> response = await ContainerUsers.ReadItemAsync<AppUser>(id, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Gets a user by its email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Found user</returns>
        /// <exception cref="ArgumentNullException">If email is null or empty</exception>
        public async Task<AppUser?> FindUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            AppUser? user = null;

            // Get LINQ IQueryable object
            IOrderedQueryable<AppUser> queryable = this.ContainerUsers.GetItemLinqQueryable<AppUser>();

            // Construct LINQ query
            var matches = queryable
                .Where(p => p.Email.Equals(email));

            // Convert to feed iterator
            using FeedIterator<AppUser> linqFeed = matches.ToFeedIterator();

            // Iterate query result pages
            while (linqFeed.HasMoreResults)
            {
                FeedResponse<AppUser> response = await linqFeed.ReadNextAsync();

                // Iterate query results
                foreach (AppUser item in response)
                {
                    return item;
                }
            }

            return user;
        }

        /// <summary>
        /// Creates a new user in the users container
        /// </summary>
        /// <param name="user"></param>
        public Task InsertUserAsync(AppUser user)
        {
            return ContainerUsers.CreateItemAsync(user);
        }

        #endregion

        #region HEROES

        /// <summary>
        /// Get all heroes from the container
        /// </summary>
        public async Task<List<Hero>> GetHeroesAsync()
        {
            List<Hero> results = [];
            // LINQ query generation
            using FeedIterator<Hero> setIterator = ContainerHeroes.GetItemQueryIterator<Hero>();
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
            ItemResponse<Hero> response = await ContainerHeroes.ReadItemAsync<Hero>(id, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Creates a new hero in the container
        /// </summary>
        /// <param name="hero"></param>
        public Task InsertHeroAsync(Hero hero)
        {
            return ContainerHeroes.CreateItemAsync(hero);
        }

        /// <summary>
        /// Updates or inserts a hero form the heroes container and returns it
        /// </summary>
        /// <param name="hero"></param>
        /// <returns>The updated or inserted hero</returns>
        public async Task<Hero> UpdateHeroAsync(Hero hero)
        {
            var response = await ContainerHeroes.UpsertItemAsync<Hero>(hero, new PartitionKey(hero.Id.ToString()));
            return response.Resource;
        }

        /// <summary>
        /// Deletes a hero from the container
        /// </summary>
        /// <param name="id"></param>
        public Task DeleteHeroAsync(string id)
        {
            return ContainerHeroes.DeleteItemAsync<Hero>(id, new PartitionKey(id));
        }

        #endregion

        #region POWERS

        /// <summary>
        /// Retrieves all powers from powers container
        /// </summary>
        /// <returns>Generic list of Powers</returns>
        public async Task<List<Power>> GetPowersAsync()
        {
            List<Power> results = [];
            // LINQ query generation
            using FeedIterator<Power> setIterator = ContainerPowers.GetItemQueryIterator<Power>();
            //Asynchronous query executiCn
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
            ItemResponse<Power> response = await ContainerPowers.ReadItemAsync<Power>(id, new PartitionKey(id));
            return response.Resource;
        }

        /// <summary>
        /// Inserts a new power to the powers container
        /// </summary>
        /// <param name="power"></param>
        public Task InsertPowerAsync(Power power)
        {
            return ContainerPowers.CreateItemAsync(power);
        }

        /// <summary>
        /// Updates or inserts a new power from the powers container and returns it
        /// </summary>
        /// <param name="power"></param>
        /// <returns>The updated or inserted power</returns>
        public async Task<Power> UpdatePowerAsync(Power power)
        {
            var response = await ContainerPowers.UpsertItemAsync(power);
            return response.Resource;
        }

        /// <summary>
        /// Deletes a power from the powers container by its id
        /// </summary>
        /// <param name="id"></param>
        public Task DeletePowerAsync(string id)
        {
            return ContainerPowers.DeleteItemAsync<Power>(id, new PartitionKey(id));
        }

        #endregion
    }
}
