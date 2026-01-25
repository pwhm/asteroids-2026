namespace Modules.User.Implementation.Handlers
{
    internal sealed class SessionHandler : ISessionHandler
    {
        private RoundState _state;

        public int RoundNumber => _state.Properties[RoundStateProperty.Round];

        public void StartNewRound()
        {
            _state.IncrementRoundCounter();
        }

        public void AddPoints(int points)
        {
            _state.AddPoints(points);
        }

        internal void StartNewSession()
        {
            _state = new RoundState();
        }
    }
}