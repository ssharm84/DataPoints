using DataPoints.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DataPoints.Repository
{
    public class DataPoint : IDataPoint
    {
        public async Task<List<DataPointsModel>> GetDataPoints()
        {
            var cosmosUrl = "https://cosmos-ss.documents.azure.com:443/";
            var cosmoskey = "IX8fKzB4ZqRDVVde7YSdDn14iDW7RVXUOckgD5oynkc0VQBS3JpLLYdNCUbBH4PHRoXtsUNY7qqIeDArxAtaag==";
            var databaseName = "CarDB";

            CosmosClient client = new CosmosClient(cosmosUrl, cosmoskey);
            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            //Container container = await database.CreateContainerIfNotExistsAsync(
            //    "CarDB", "/partitionKeyPath", 400);

            //dynamic testItem = new { id = Guid.NewGuid().ToString(), partitionKeyPath = "MyTestPkValue", details = "it's working" };
            //ItemResponse<dynamic> response = await container.CreateItemAsync(testItem);
            Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/category",
            throughput: 400
            );
            var query = new QueryDefinition(
                query: "SELECT * FROM CarDB p WHERE p.partitionKey = @key"
                )
                .WithParameter("@key", "category");

            using FeedIterator<DataPointsModel> feed = container.GetItemQueryIterator<DataPointsModel>(
                queryDefinition: query);

            while (feed.HasMoreResults)
            {
                FeedResponse<DataPointsModel> response = await feed.ReadNextAsync();
                foreach (DataPointsModel item in response)
                {
                    Console.WriteLine($"Found item:\t{item.Category}");
                }

            }
            return feed;
        }
    }
}
