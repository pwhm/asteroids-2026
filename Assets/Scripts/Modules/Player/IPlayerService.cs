using Core.Services;

namespace Modules.Player
{
    public interface IPlayerService : IService
    {
        void SetupForNewRound();
        void StartRound();
        void FinishRound();
    }
}