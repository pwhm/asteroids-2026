namespace Modules.User.Implementation.Handlers
{
    internal sealed class SessionHandler : ISessionHandler
    {
        private RoundState _state;

        public int RoundNumber => _state.Properties[RoundStateProperty.Round];

        public void StartNewRound()
        {
            _state.Properties[RoundStateProperty.Round] += 1;
        }

        public void AddPoints(int points)
        {
            _state.Properties[RoundStateProperty.Score] += points;
        }

        public void DeductLife()
        {
            _state.Properties[RoundStateProperty.Lives] -= 1;
        }

        internal void StartNewSession()
        {
            _state = new RoundState();
        }
    }
}