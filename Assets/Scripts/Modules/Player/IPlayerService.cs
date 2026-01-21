namespace Modules.Player
{
    public interface IPlayerService
    {
        public void Initialize();
        void SetupForNewRound();
        void StartRound();
        void FinishRound();
    }
}