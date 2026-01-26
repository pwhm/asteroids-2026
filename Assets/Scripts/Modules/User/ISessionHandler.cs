namespace Modules.User
{
    public interface ISessionHandler
    {
        public int RoundNumber { get; }
        public int Lives { get; }
        
        public void StartNewRound();
        public void AddPoints(int points);
        public void DeductLife();
    }
}