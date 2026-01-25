using Core.Services;

namespace Modules.User
{
    public interface IUserSessionStateService : IService
    {
        public ISessionHandler Session { get; }
        
        public void StartNewSession();
    }
}