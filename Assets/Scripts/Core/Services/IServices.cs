namespace Core.Services
{
    public interface IServices
    {
        public void AddService<T>(T service, ServiceScope scope) where T : IService;
        public void RemoveLocalServices();
        public void GetService<T>() where T : IService;
    }
}