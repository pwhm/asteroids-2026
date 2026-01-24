using System.Collections.Generic;
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
        
        private ProjectileController _projectilePrefab;
        private ObjectPool<ProjectileController> _projectilePool;
        private List<ProjectileController> _activeProjectiles = new();

        public ProjectileHandler(InputSystem_Actions inputActions, ProjectileController projectilePrefab)
        {
            _inputActions = inputActions;
            _projectilePrefab = projectilePrefab;
            
            _projectilePool = new ObjectPool<ProjectileController>(CreateProjectile, actionOnRelease:OnProjectileReleased);
        }

        public void Update()
        {
            if (_playerTransform == null || _inputActions == null)
            {
                return;
            }
            
            if (_inputActions.Player_1.Fire.WasPerformedThisFrame())
            {
                OnProjectileRequested(_playerTransform.up, _playerTransform.position);
            }
        }
        
        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
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