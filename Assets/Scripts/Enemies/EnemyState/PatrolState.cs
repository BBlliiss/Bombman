using UnityEngine;

namespace Enemies
{
    public class PatrolState:EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            enemy.animState = 0;
            enemy.SwitchTargetPoint();
            enemy.CheckFacingDir();
        }

        public override void Update(Enemy enemy)
        {
            if(!enemy.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                enemy.animState = 1;
                enemy.MoveToTarget();
            }

            if (Vector2.Distance(enemy.targetPoint.position, enemy.transform.position) < .1f)
            {
                enemy.ChangeToState(enemy.patrolState);
            }

            if (enemy.targets.Count > 0)
            {
                enemy.ChangeToState(enemy.attackState);
            }
        }
    }
}