using Core.Services;
using Modules.Common;
using Modules.User;
using TMPro;
using UnityEngine;

namespace Modules.Gameplay.Implementation.View
{
    internal class GameplayView : MonoBehaviour
    {
        private IUserSessionStateService _userSessionStateService => Services.GetService<IUserSessionStateService>();
        
        [SerializeField] private TMP_Text _livesLabel;

        private void Awake()
        {
            _livesLabel.text = string.Empty;
        }
        
        private void OnEnable()
        {
            Events.Gameplay.RoundStarted += OnRoundStarted;
            Events.Gameplay.PlayerLostLife +=  RefreshLifeCounter;
        }

        private void OnDisable()
        {
            Events.Gameplay.RoundStarted -= OnRoundStarted;
            Events.Gameplay.PlayerLostLife -=  RefreshLifeCounter;
        }

        private void OnRoundStarted()
        {
            RefreshLifeCounter();
        }

        private void RefreshLifeCounter()
        {
            _livesLabel.text = _userSessionStateService.Session.Lives.ToString();
        }
    }
}