using AISQLGenerator.AITasks;
using AISQLGenerator.SQLTasks;
using System.Text;

namespace AI_SQL_Generator.AIPrompts
{
    public class AIPrompt
    {
        public AIPrompt()
        {

        }

        public void ImplementPrompt(AzureSQL azureSQL, AIActions aIActions, string databaseInfo)
        {
            Console.WriteLine("What query do you want generated:");
            string userInput = Console.ReadLine();

            string prompt = GeneratePrompt(databaseInfo, userInput);

            string response = aIActions.Ask(databaseInfo);
            Console.WriteLine("\n\nGenerated SQL Query:\n" + response + "\n");

            if (azureSQL.IsSqlQueryValid(response))
            {
                azureSQL.RunQuery(response);
            }
            else
            {
                Console.WriteLine("SQL Query Generated Invalid, Please try another prompt.");
                ImplementPrompt(azureSQL, aIActions, databaseInfo);
            }
        }


        public string GenerateDatabaseInfo(AzureSQL azureSQL)
        {
            StringBuilder databaseInfoBuilder = new StringBuilder();
            databaseInfoBuilder.AppendLine("Database Version:");
            databaseInfoBuilder.AppendLine(azureSQL.GetDBVersion());
            databaseInfoBuilder.AppendLine("Tables:");
            List<string> tableNames = azureSQL.GetTableNames();
            foreach (var tableName in tableNames)
            {
                databaseInfoBuilder.AppendLine(tableName);
                List<string> columnNames = azureSQL.GetColumnNames(tableName);
                foreach (var columnName in columnNames)
                {
                    databaseInfoBuilder.AppendLine("- " + columnName);
                }
                databaseInfoBuilder.AppendLine();
            }
            databaseInfoBuilder.AppendLine("Foreign Keys:");
            databaseInfoBuilder.AppendLine(azureSQL.GetForeignKeys());

            return databaseInfoBuilder.ToString();
        }

        public string GeneratePrompt(string databaseInfo, string userPrompt)
        {
            StringBuilder promptBuilder = new StringBuilder();
            promptBuilder.AppendLine(AIInstruction());
            promptBuilder.AppendLine("Schema:");
            promptBuilder.AppendLine(databaseInfo);
            promptBuilder.AppendLine("User Input: " + userPrompt + "\n");
            promptBuilder.AppendLine(ClarificationInfo());
            promptBuilder.AppendLine("Now generate the SQL query:");
            return promptBuilder.ToString();
        }

        public string AIInstruction()
        {
            return "You are a SQL generator. Only output valid SQL SELECT statements and nothing else.\n";
        }

        public string ClarificationInfo()
        {
            return "Important Instructions:\n"
                + "- Only output a valid SQL SELECT query that answers the question exactly.\n"
                + "- Use COUNT and GROUP BY if the query logic requires identifying duplicates or aggregations.\n"
                + "- Use JOINs to combine data from multiple tables when necessary.\n"
                + "- Do not return any text, explanation, formatting, or code block markers."
                + "- Use fully qualified column names where ambiguity may exist\n"
                + "- Output must be plain SQL.\n";
        }
    }
}
