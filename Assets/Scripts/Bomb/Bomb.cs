using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private float startTime;
    public PlayerController player;

    [Header("Bomb Settings")] public float bombForce;
    public float duration;
    public LayerMask targetLayer;

    [Header("Explosion Check")] public Transform explosion;
    public float radius;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        startTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > startTime + duration)
        {
            anim.Play("BombExplode");
            rb.velocity = Vector2.zero;
        }
    }

    public void Explode()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(explosion.position, radius, targetLayer);

        foreach (var target in targets)
        {
            if (target == GetComponent<Collider2D>())
                continue;

            Vector3 dir = (target.transform.position - explosion.position).normalized;
            
            if(target.GetComponent<Rigidbody2D>() != null)
                target.GetComponent<Rigidbody2D>().AddForce(dir * bombForce, ForceMode2D.Impulse);
        }
    }

    public void AnimationFinish()
    {
        Destroy(gameObject);
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(explosion.position, radius);
    }

    #endregion
}