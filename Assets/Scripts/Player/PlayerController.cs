using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Rigidbody2D rb;
    public Animator anim;

    [Header("Player Settings")] public float speed;
    public float jumpForce;

    [Header("Stats")] public int maxHp;
    public StatInt currentHp;
    public bool isDead = false;

    [Header("Ground Check")] public LayerMask groundLayer;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public float groundCheckDistance;

    [Header("FX")] public Transform jumpFX;
    public Transform fallFX;
    public GameObject JumpFXPrefab;
    public GameObject FallFXPrefab;

    [Header("Attack Settings")] public GameObject bombPrefab;
    public float attackCD;
    private float attackTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        currentHp = new StatInt(maxHp, _onValueDecreased: Hurt, _onValueNegative: Dead);
    }

    private void Update()
    {
        if (isDead) return;

        CheckInput();

        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isAir", !isGroundDetected());
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        Move();
    }

    #region Input

    private void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGroundDetected())
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
    }

    #endregion

    #region PlayerMovements

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput != 0)
            transform.localScale = new Vector3(horizontalInput, 1, 1);
    }

    private void Jump()
    {
        StartJumpFX();
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void Attack()
    {
        if (Time.time > attackTimer)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);

            attackTimer = Time.time + attackCD;
        }
    }

    public void Hurt()
    {
        anim.SetTrigger("Hurt");
    }

    public void Dead()
    {
        isDead = true;
        anim.SetBool("Dead", isDead);
    }

    #endregion

    #region PhysicsCheck

    public bool isGroundDetected()
    {
        return Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    #endregion

    #region FX

    public void StartJumpFX()
    {
        Instantiate(JumpFXPrefab, jumpFX.position, Quaternion.identity);
    }

    public void StartFallFX()
    {
        Instantiate(FallFXPrefab, fallFX.position, Quaternion.identity);
    }

    #endregion

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck1.transform.position, groundCheck1.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(groundCheck2.transform.position, groundCheck2.position + Vector3.down * groundCheckDistance);
    }

    #endregion

    public void GetHurt(int damage)
    {
        if(anim.GetCurrentAnimatorStateInfo(1).IsName("Hurt")) return;
        
        currentHp.SetValue(currentHp - damage);
    }
}