using System.Threading.Tasks;
using Modules.User.Implementation.Handlers;

namespace Modules.User.Implementation
{
    internal sealed class UserService : IUserService
    {
        public ISessionHandler Session => _sessionHandler;
        
        private SessionHandler _sessionHandler;
        
        public Task InitializeAsync()
        {
            _sessionHandler = new SessionHandler();
            return Task.CompletedTask;
        }

        public void StartNewSession()
        {
            _sessionHandler.StartNewSession();
        }
    }
}