using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Base Parameter")] public float speed;
    public float jumpForce;

    [Header("Ground Check")] public LayerMask groundLayer;
    public Transform groundCheck1;
    public Transform groundCheck2;
    public float groundCheckDistance;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Jump") && isGroundDetected())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput != 0)
            transform.localScale = new Vector3(horizontalInput, 1, 1);
    }

    public bool isGroundDetected()
    {
        return Physics2D.Raycast(groundCheck1.position, Vector2.down, groundCheckDistance, groundLayer) ||
               Physics2D.Raycast(groundCheck2.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    #region Gizmos

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck1.transform.position, groundCheck1.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(groundCheck2.transform.position, groundCheck2.position + Vector3.down * groundCheckDistance);
    }

    #endregion
}