

namespace KristinaBot.BL.Abstracts.ServicesAbstracts
{
    public interface ICommandIdentifierService
    {
        Task<string[]> IdentifyRequest (string message);
    }
}
