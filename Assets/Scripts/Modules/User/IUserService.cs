using System.Threading.Tasks;
using Core.Services;
using Modules.User;

namespace Modules.Progression
{
    public interface IProgressionService : IService, IUserProgressionService, IUserRoundStateService
    {
    }
}