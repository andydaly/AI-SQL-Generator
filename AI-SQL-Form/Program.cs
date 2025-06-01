using AISQLGenerator.AITasks;
using Microsoft.Extensions.Configuration;

namespace AI_SQL_Form
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration configuration = builder.Build();
            string endpoint = configuration["ApiSettings:Endpoint"];
            string apiKey = configuration["ApiSettings:ApiKey"];
            string deploymentName = configuration["ApiSettings:DeploymentName"];
            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            AzureOpenAI azureOpenAI = new AzureOpenAI(endpoint, apiKey, deploymentName);
            if (!string.IsNullOrEmpty(connectionString))
                azureOpenAI.ConnectionString = connectionString;

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(azureOpenAI));
        }
    }
}