namespace Core.Services
{
    internal sealed class Services : IServices
    {
        public void AddService<T>(T service, ServiceScope scope) where T : IService
        {
            throw new System.NotImplementedException();
        }

        public void RemoveLocalServices()
        {
            throw new System.NotImplementedException();
        }

        public void GetService<T>() where T : IService
        {
            throw new System.NotImplementedException();
        }
    }
}