using System;
using UnityEngine;

namespace Enemies
{
    public class BigGuyAnimationTrigger : MonoBehaviour
    {
        private BigGuy enemy;

        private void Start()
        {
            enemy = GetComponentInParent<BigGuy>();
        }

        public void ThrowBomb()
        {
            enemy.ThrowBomb();
        }
    }
}