using System.Collections.Generic;

namespace Modules.User.Implementation
{
    internal enum RoundStateProperty
    {
        Round = 0,
        Score = 1,
        
        Lives = 10,
    }
    
    internal sealed class RoundState
    {
        public Dictionary<RoundStateProperty, int> Properties = new();
        
        public RoundState()
        {
            Properties[RoundStateProperty.Round] = 0;
            Properties[RoundStateProperty.Score] = 0;

            Properties[RoundStateProperty.Lives] = 5;
        }

        public void IncrementRoundCounter()
        {
            Properties[RoundStateProperty.Round] += 1;
        }

        public void AddPoints(int points)
        {
            Properties[RoundStateProperty.Score] += points;
        }
        
    }
}