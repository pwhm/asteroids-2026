using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Services
{
    internal static class Services
    {
        private static Dictionary<Type, IService> _services = new();
        private static List<Type> _sceneServices = new();
        
        public static void AddService<T>(T service, ServiceScope scope) where T : IService
        {
            if (_services.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"#Core# Failed to add service: Service {typeof(T)} already exists.");
            }
            
            _services.Add(typeof(T), service);
            if (scope == ServiceScope.Scene)
            {
                _sceneServices.Add(typeof(T));
            }
        }

        public static void RemoveSceneServices()
        {
            foreach (var service in _sceneServices)
            {
                _services.Remove(service);
            }
            
            _sceneServices.Clear();
        }

        public static T GetService<T>() where T : IService
        {
            if (!_services.TryGetValue(typeof(T), out var service))
            {
                throw new KeyNotFoundException($"#Core# No service found for type {typeof(T)}");
            }

            return (T)service;
        }

        public static void CleanUp()
        {
            _sceneServices.Clear();
            _services.Clear();
        }
    }
}