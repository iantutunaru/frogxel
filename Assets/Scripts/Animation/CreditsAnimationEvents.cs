using System;
using UnityEngine;

namespace Frogxel.Animation
{
    public class CreditsAnimationEvents : MonoBehaviour
    {
        public event Action CreditsEnd;
        
        public void HandleCreditsEnd()
        {
            CreditsEnd?.Invoke();
        }
    }
}