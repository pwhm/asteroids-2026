using System.Threading.Tasks;
using Core.Services;

namespace Modules.Asteroids.Implementation
{
    public static class AsteroidsModuleInstaller
    {
        public static Task InstallAsync()
        {
            Services.AddService<IAsteroidsService>(new AsteroidsService(), ServiceScope.Scene);
            
            return  Task.CompletedTask;
        }

        public static async Task InitializeAsync()
        {
            await Services.GetService<IAsteroidsService>().InitializeAsync();
        }
    }
}