using AI_SQL_Generator.AIPrompts;
using AISQLGenerator.AITasks;
using AISQLGenerator.SQLTasks;
using Microsoft.Extensions.Configuration;


class Program
{

    static async Task Main(string[] args)
    {


        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        IConfiguration configuration = builder.Build();
        string endpoint = configuration["ApiSettings:Endpoint"];
        string apiKey = configuration["ApiSettings:ApiKey"];
        string deploymentName = configuration["ApiSettings:DeploymentName"];
        string connectionString = configuration["ConnectionStrings:DefaultConnection"];

        AzureSQL azureSQL = new AzureSQL(connectionString);
        AIPrompt aiPrompt = new AIPrompt();

        string databaseInfo = aiPrompt.GenerateDatabaseInfo(azureSQL);
        Console.WriteLine(databaseInfo);

        AzureOpenAI azureOpenAI = new AzureOpenAI(endpoint, apiKey, deploymentName);
        AIActions aIActions = new AIActions(azureOpenAI);

        aiPrompt.ImplementPrompt(azureSQL, aIActions, databaseInfo);
    }
}