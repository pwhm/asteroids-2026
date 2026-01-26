using System.Collections.Generic;
using UnityEngine;

namespace Modules.Gameplay.Implementation.View
{
    public class LifeCounterController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _lives;

        public void Refresh(int lives)
        {
            for (var i = 0; i < _lives.Count; i++)
            {
                _lives[i].SetActive(i < lives);
            }
        }
    }
}