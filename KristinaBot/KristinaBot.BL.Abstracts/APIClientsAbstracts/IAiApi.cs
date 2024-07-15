using KristinaBot.DataObjects.AI;

namespace KristinaBot.BL.Abstracts.APIClientsAbstracts
{
    public interface IAiApi
    {
        Task<AIResponse> HandleMessage(string message);
    }
}
