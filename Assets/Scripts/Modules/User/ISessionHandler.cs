namespace Modules.User
{
    public interface ISessionHandler
    {
        public int RoundNumber { get; }
        public void StartNewRound();
        public void AddPoints(int points);
    }
}