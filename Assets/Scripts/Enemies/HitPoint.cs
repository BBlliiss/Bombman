using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponentInParent<Enemy>().Attack(other.GetComponent<IDamageable>());
        }
        else if (other.CompareTag("Bomb"))
        {
            GetComponentInParent<Enemy>().Skill(other);
        }
    }
}
