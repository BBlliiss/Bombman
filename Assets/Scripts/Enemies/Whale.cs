using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Whale : Enemy
    {
        [Header("Skill Settings")] public Transform bombPoint;
        public float growScale;
        public int maxSwallowAmount;
        public float launchForce;

        public List<GameObject> bombs = new();

        public override void Skill(Behaviour item)
        {
            if (bombs.Count >= maxSwallowAmount) return;
            Swallow(item);
        }

        public override void Dead()
        {
            base.Dead();

            ReleaseAllBombs();
        }

        private void Swallow(Behaviour item)
        {
            item.GetComponent<Bomb>().TurnOff();
            item.gameObject.SetActive(false);
            bombs.Add(item.gameObject);

            Bigger();

            if (bombs.Count == maxSwallowAmount)
            {
                Invoke("Dead", .5f);
            }
        }

        private void Bigger()
        {
            transform.localScale *= growScale;
            attackRange *= growScale;
            skillRange *= growScale;
        }

        private void ReleaseAllBombs()
        {
            for (int i = bombs.Count - 1; i >= 0; i--)
            {
                GameObject bomb = bombs[i];
                bomb.transform.position = bombPoint.position;
                bomb.SetActive(true);
                bomb.GetComponent<Bomb>().TurnOn(Random.Range(0f,1f));

                float rad = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                bomb.GetComponent<Rigidbody2D>()
                    .AddForce(new Vector2(launchForce * Mathf.Cos(rad), launchForce * Mathf.Sin(rad)),
                        ForceMode2D.Impulse);

                bombs.Remove(bomb);
            }
        }
    }
}