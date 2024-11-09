using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy
{
    [Header("Skill Settings")] public Vector2 kickForce;

    public override void Skill(Behaviour item)
    {
        item.GetComponent<Rigidbody2D>()
            ?.AddForce(new Vector2(kickForce.x * facingDir, kickForce.y), ForceMode2D.Impulse);
    }

    public override void Attack(Behaviour _target)
    {
        base.Attack(_target);
        _target.GetComponent<Rigidbody2D>()
            ?.AddForce(new Vector2(kickForce.x * facingDir, kickForce.y * 4), ForceMode2D.Impulse);
    }
}