using System.Threading.Tasks;

namespace Core.Services
{
    public interface IService
    {
        Task InitializeAsync();
    }
}