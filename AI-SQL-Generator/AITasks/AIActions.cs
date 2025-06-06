﻿using OpenAI.Chat;

namespace AISQLGenerator.AITasks
{
    public class AIActions
    {
        AzureOpenAI AzureOpenAI { get; set; }
        public AIActions(AzureOpenAI azureOpenAI)
        {
            AzureOpenAI = azureOpenAI;
        }

        public string Ask(string questionText)
        {

            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a SQL query generator. You respond ONLY with valid SQL SELECT statements. Do not include explanations, formatting, or additional text. Do not include code blocks, markdown, or anything else."),
                new UserChatMessage(questionText),
            };

            var response = AzureOpenAI.ChatClient.CompleteChat(messages);
            return response.Value.Content[0].Text;
        }

        public void MultiTurnAsk(string questionText)
        {
            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage("I am going to Paris, what should I see?"),
            };
            var response = AzureOpenAI.ChatClient.CompleteChat(messages);
            System.Console.WriteLine(response.Value.Content[0].Text);
            messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));
            // Append new user question.
            messages.Add(new UserChatMessage("What is so great about #1?"));

            response = AzureOpenAI.ChatClient.CompleteChat(messages);
            System.Console.WriteLine(response.Value.Content[0].Text);
        }

        public void StreamAsk(string questionText)
        {
            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage("I am going to Paris, what should I see?"),
            };

            var response = AzureOpenAI.ChatClient.CompleteChatStreaming(messages);

            foreach (StreamingChatCompletionUpdate update in response)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    System.Console.Write(updatePart.Text);
                }
            }
            System.Console.WriteLine("");
        }
    }
}
