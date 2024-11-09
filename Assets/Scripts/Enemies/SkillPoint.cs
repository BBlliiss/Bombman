using UnityEngine;

namespace Enemies
{
    public class SkillPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Bomb"))
            {
                GetComponentInParent<Enemy>().Skill(other);
            }
        }
    }
}