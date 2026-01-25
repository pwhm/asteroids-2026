using System.Threading.Tasks;
using Core.Services;

namespace Modules.User.Implementation
{
    public static class ProgressionModuleInstaller
    {
        public static void InstallProjectServices()
        {
            var instance = new UserService();
            
            Services.AddService<IUserService>(instance, ServiceScope.Global);
            Services.AddService<IUserProgressionService>(instance, ServiceScope.Global);
            Services.AddService<IUserSessionStateService>(instance, ServiceScope.Global);
        }

        public static async Task InitializeProjectServicesAsync()
        {
            await Services.GetService<IUserService>().InitializeAsync();
        }
    }
}