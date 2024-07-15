using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.DataObjects.AI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KristinaBot.BL.Services
{
    public class AiApiClient : IAiApi
    {
        private readonly string aiApiUrl;
        private readonly string apiKey;
        public AiApiClient(IConfiguration configuration)
        {
            IConfigurationSection apiSection = configuration.GetSection("API");
            aiApiUrl = apiSection.GetSection("AiApi").Value;
            apiKey = apiSection.GetSection("AiApiKey").Value;
        }

        public async Task<AIResponse> HandleMessage(string message)
        {
            using HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(aiApiUrl)
            };

            client.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", apiKey);
            var fullMessage = $"Identify the topic of the question at the end of the text (weather or exchange rates). If it is weather, send the answer in the form of the line: 'weather / (location name) / (latitude of the location from the question) / (longitude of the location from the question)', if this is the exchange rate, send the answer in the form of the line: 'exchange / (international code of the currency you need transfer) / (international code of the currency to which you want to convert) / (amount, if indicated in the question or number 1 if not indicated)'. If the question does not relate to the proposed topics, send the number 0. If the question requires clarification, send the number 1. Question: {message} ";
            var request = new AIRequest { model = "gpt-3.5-turbo", messages = new List<Message> { new Message { role ="user", content = fullMessage } } };
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("chat/completions", request);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AIResponse>(result);
            }
            catch(Exception ex)
            {
                return new AIResponse();
            }
        }
    }
}
