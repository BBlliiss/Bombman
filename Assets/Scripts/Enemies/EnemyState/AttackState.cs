using UnityEngine;

namespace Enemies
{
    public class AttackState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            enemy.animState = 2;
        }

        public override void Update(Enemy enemy)
        {
            switch (enemy.targets.Count)
            {
                case <= 0:
                    enemy.ChangeToState(enemy.patrolState);
                    break;
                case 1:
                    enemy.targetPoint = enemy.targets[0];
                    break;
                default:
                {
                    if (enemy.targetPoint != null)
                    {
                        float tempDis = Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x);
                        for (int i = 1; i < enemy.targets.Count; i++)
                        {
                            if (Mathf.Abs(enemy.transform.position.x - enemy.targets[i].position.x) < tempDis)
                            {
                                enemy.targetPoint = enemy.targets[i];
                                tempDis = Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x);
                            }
                        }
                    }

                    break;
                }
            }

            if (enemy.targetPoint != null)
            {
                if (enemy.targetPoint.CompareTag("Player"))
                    enemy.AttackAction();
                else if (enemy.targetPoint.CompareTag("Bomb"))
                    enemy.SkillAction();
            }

            enemy.MoveToTarget();
        }
    }
}