using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public float bombTimer;
    public bool isOn;

    [Header("Bomb Settings")] public int damage;
    public float bombForce;
    public float duration;
    public LayerMask targetLayer;

    [Header("Explosion Check")] public Transform explosion;
    public float radius;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        TurnOn();
    }

    private void Update()
    {
        if (isOn && bombTimer >= duration)
        {
            anim.Play("BombExplode");
            rb.velocity = Vector2.zero;
        }

        bombTimer += Time.deltaTime;
    }

    public void Explode()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(explosion.position, radius, targetLayer);

        foreach (var target in targets)
        {
            if (target == GetComponent<Collider2D>())
                continue;

            Vector3 dir = (target.transform.position - explosion.position).normalized;

            if (target.GetComponent<Rigidbody2D>() != null)
                target.GetComponent<Rigidbody2D>().AddForce(dir * bombForce, ForceMode2D.Impulse);

            if (target.CompareTag("Bomb"))
            {
                target.GetComponent<Bomb>().TurnOn();
            }

            if (target.GetComponent<IDamageable>() != null)
            {
                target.GetComponent<IDamageable>().GetHurt(damage);
            }
        }
    }

    public void TurnOff()
    {
        if (!isOn) return;

        isOn = false;
        anim.Play("BombOff");
        gameObject.layer = LayerMask.NameToLayer("Environment");
    }

    public void TurnOn(float startTimer = 0)
    {
        if (isOn) return;

        isOn = true;
        anim.Play("BombOn");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
        bombTimer = startTimer;
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