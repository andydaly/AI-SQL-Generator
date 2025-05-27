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
        //azureSQL.RunQuery("SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'");

        string prompt = "";
        prompt = "Tables:\n";
        List<string> tableNames = azureSQL.GetTableNames();
        foreach (var tableName in tableNames)
        {
            //Console.WriteLine(tableName);
            prompt = prompt + tableName + "\n";
            List<string> columnNames = azureSQL.GetColumnNames(tableName);
            foreach (var columnName in columnNames)
            {
                //Console.WriteLine(columnName);
                prompt = prompt + "- " + columnName + "\n";
            }
            //Console.WriteLine();
            prompt = prompt + "\n";
        }
        //Console.WriteLine();
        //Console.WriteLine("Foreign Keys:");
        prompt = prompt + "Foreign Keys:\n";

        //Console.WriteLine(azureSQL.GetForeignKeys());
        prompt = prompt + azureSQL.GetForeignKeys() + "\n";
        Console.WriteLine(prompt);

        AzureOpenAI azureOpenAI = new AzureOpenAI(endpoint, apiKey, deploymentName);
        AIActions aIActions = new AIActions(azureOpenAI);

        Console.WriteLine("What query do you want generated:");
        string userInput = Console.ReadLine();

        prompt = prompt + "\nBased off the above Database information generate an SQL SELECT Query Statement (and no other text or extra characters also be aware of Ambiguous column names) based off the following input: \n" + userInput + "\n";

        //Console.WriteLine(prompt);

        string response = aIActions.Ask(prompt);
        Console.WriteLine("\n\nGenerated SQL Query:\n" + response + "\n");
        azureSQL.RunQuery(response);


    }
}