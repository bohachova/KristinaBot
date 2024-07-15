

using KristinaBot.BL.Abstracts.APIClientsAbstracts;
using KristinaBot.BL.Abstracts.ServicesAbstracts;

namespace KristinaBot.BL.Services
{
    public class CommandIdentifierService : ICommandIdentifierService
    {
        private readonly IAiApi aiApi;
        public CommandIdentifierService(IAiApi aiApi)
        {
            this.aiApi = aiApi;
        }

        public async Task<string[]> IdentifyRequest (string message)
        {
            var result = await aiApi.HandleMessage(message);
            var response = result.choices.FirstOrDefault()?.message?.content;
            var respArray = response.Split('/');
            for ( var i = 0; i < respArray.Length; i++ )
            {
                respArray[i] = respArray[i].Trim();
            }
            return respArray;
        }
    }
}
