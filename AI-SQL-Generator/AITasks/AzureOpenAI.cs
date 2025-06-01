using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;


namespace AISQLGenerator.AITasks
{
    public class AzureOpenAI
    {
        public AzureOpenAIClient OpenAIClient { get; private set; }
        public ChatClient ChatClient { get; private set; }

        public string ConnectionString { get; set; } = string.Empty;
        public AzureOpenAI(string endpoint, string apiKey, string deploymentName)
        {
            OpenAIClient = new(new Uri(endpoint), new AzureKeyCredential(apiKey));
            ChatClient = OpenAIClient.GetChatClient(deploymentName);
        }
    }
}
