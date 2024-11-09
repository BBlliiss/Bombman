using UnityEngine;

namespace Enemies
{
    public class Captain : Enemy
    {
        public bool isScared;

        public override void SkillAction()
        {
            if (!targetPoint.GetComponent<Bomb>().isOn) return;

            base.SkillAction();

            if (targetPoint.position.x < transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right,
                    speed * 3.2f * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left,
                    speed * 3.2f * Time.deltaTime);
            }
        }
    }
}