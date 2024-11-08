using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy
{
    public override void Skill(Behaviour item)
    {
        item.GetComponent<Bomb>()?.TurnOff();
    }
}
