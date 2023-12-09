using Microsoft.Azure.Cosmos;

namespace HeroesAPI.Helpers
{
    public class HelperCosmosDb(IConfiguration configuration, CosmosClient cosmosClient)
    {
        readonly private string? cosmosDatabasename = configuration.GetValue<string>("HeroesCosmosDB:Database");
        readonly private string? containerHeroesName = configuration.GetValue<string>("HeroesCosmosDB:ContainerHeroes");
        readonly private string? containerPowersName = configuration.GetValue<string>("HeroesCosmosDB:ContainerPowers");

        public Container GetContainerHeroes()
        {
            return cosmosClient.GetContainer(cosmosDatabasename, containerHeroesName);
        }

        public Container GetContainerPowers()
        {
            return cosmosClient.GetContainer(cosmosDatabasename, containerPowersName);
        }
    }
}
