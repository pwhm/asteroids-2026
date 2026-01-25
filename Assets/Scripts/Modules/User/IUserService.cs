using Core.Services;

namespace Modules.User
{
    public interface IUserService : IService, IUserProgressionService, IUserSessionStateService
    {
    }
}