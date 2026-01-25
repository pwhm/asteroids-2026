using Core.Services;

namespace Modules.Gameplay
{
    public interface IGameplayService : IService
    {
        /// <summary>
        /// Loads gameplay scene and starts the session
        /// </summary>
        public void SwitchTo();
        public void StartRound();
    }
}