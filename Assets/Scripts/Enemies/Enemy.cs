using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    private EnemyState currentState;
    public PatrolState patrolState = new();
    public AttackState attackState = new();

    [HideInInspector] public Animator anim;
    [HideInInspector] public int animState;

    [Header("Stats")] public int maxHp;
    public StatInt currentHp;
    public bool isDead;
    public int damage;

    [Header("Enemy Settings")] public int facingDir = 1;
    public float speed;

    [Header("Navigation")] public Transform pointA;
    public Transform pointB;
    [HideInInspector] public Transform targetPoint;

    public List<Transform> targets = new();

    [Header("Attack Settings")] public float attackCD;
    public float skillCD;
    public float attackRange, skillRange;
    private float attackTimer, skillTimer;

    [Header("FX")] public GameObject alarmSign;

    protected virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        animState = 0;
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        currentHp = new StatInt(maxHp, _onValueDecreased: Hurt, _onValueNegative: Dead);

        ChangeToState(patrolState);
    }

    private void Update()
    {
        if (isDead) return;

        currentState.Update(this);
        anim.SetInteger("State", animState);
    }

    public void ChangeToState(EnemyState state)
    {
        currentState = state;
        currentState.Enter(this);
    }

    #region Actions

    public virtual void MoveToTarget()
    {
        Vector2 targetPos = new Vector2(targetPoint.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        CheckFacingDir();
    }

    public void AttackAction()
    {
        if (Vector2.Distance(transform.position, targetPoint.position) < attackRange && Time.time > attackTimer)
        {
            anim.SetTrigger("Attack");
            attackTimer = Time.time + attackCD;
        }
    }

    public virtual void SkillAction()
    {
        if (!targetPoint.GetComponent<Bomb>().isOn) return;

        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange && Time.time > skillTimer)
        {
            anim.SetTrigger("Skill");
            skillTimer = Time.time + skillCD;
        }
    }

    public virtual void GetHurt(int _damage)
    {
        if (anim.GetCurrentAnimatorStateInfo(2).IsName("Hurt")) return;

        currentHp.SetValue(currentHp - _damage);
    }

    public virtual void Hurt()
    {
        anim.SetTrigger("Hurt");
    }

    public virtual void Dead()
    {
        isDead = true;
        anim.SetBool("Dead", isDead);
    }

    public virtual void Attack(Behaviour _target)
    {
        _target.GetComponent<IDamageable>()?.GetHurt(damage);
    }

    public virtual void Skill(Behaviour item)
    {
    }

    public void CheckFacingDir()
    {
        if ((facingDir == -1 && transform.position.x < targetPoint.position.x) ||
            (facingDir == 1 && transform.position.x > targetPoint.position.x))
        {
            facingDir *= -1;
            transform.Rotate(0, 180, 0);
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && !alarmSign.activeSelf)
        {
            StartCoroutine(OnAlarm());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!targets.Contains(other.transform))
        {
            targets.Add(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (targets.Contains(other.transform))
        {
            targets.Remove(other.transform);
        }
    }

    private IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip
            .length);
        alarmSign.SetActive(false);
    }

    public void SwitchTargetPoint()
    {
        targetPoint = Vector2.Distance(pointA.position, transform.position) >
                      Vector2.Distance(pointB.position, transform.position)
            ? pointA
            : pointB;
    }
}