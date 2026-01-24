using System.Collections.Generic;
using Core;
using Modules.Common;
using UnityEngine;
using UnityEngine.Pool;

namespace Modules.Player.Implementation.Handlers
{
    // Sperate module?
    internal sealed class ProjectileHandler
    {
        private InputSystem_Actions _inputActions;
        private Transform _playerTransform;
        
        private ScreenBounds _screenBounds;
        
        private ProjectileController _projectilePrefab;
        private ObjectPool<ProjectileController> _projectilePool;
        private List<ProjectileController> _activeProjectiles = new();

        public ProjectileHandler(InputSystem_Actions inputActions, ProjectileController projectilePrefab)
        {
            _inputActions = inputActions;
            _projectilePrefab = projectilePrefab;
            
            _projectilePool = new ObjectPool<ProjectileController>(
                CreateProjectile, 
                (element) => element.gameObject.SetActive(true),
                OnProjectileReleased);
            _screenBounds = ScreenHelper.GetScreenBounds(Camera.main);
        }

        public void Update()
        {
            if (_playerTransform == null || _inputActions == null)
            {
                return;
            }

            ReleaseProjectilesOffScreen();
            
            if (_inputActions.Player_1.Fire.WasPerformedThisFrame())
            {
                OnProjectileRequested(_playerTransform.up, _playerTransform.position);
            }
        }
        
        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
        }

        private void ReleaseProjectilesOffScreen()
        {
            var projectilesToRelease = new List<ProjectileController>();
            foreach (var projectile in _activeProjectiles)
            {
                if (_screenBounds.IsWithinBounds(projectile.transform.position))
                {
                    continue;
                }
                
                projectilesToRelease.Add(projectile);
            }

            foreach (var projectile in projectilesToRelease)
            {
                _projectilePool.Release(projectile);
            }
            projectilesToRelease.Clear();
        }

        private void OnProjectileRequested(Vector3 playerDirection, Vector3 playerPosition)
        {
            var projectile = _projectilePool.Get();
            
             projectile.Initialize(playerDirection, playerPosition);
             _activeProjectiles.Add(projectile);
        }

        private ProjectileController CreateProjectile()
        {
            var instance = Object.Instantiate(_projectilePrefab);

            return instance;
        }

        private void OnProjectileReleased(ProjectileController projectile)
        {
            projectile.gameObject.SetActive(false);
            _activeProjectiles.Remove(projectile);
        }
    }
}