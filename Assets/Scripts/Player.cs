using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10.0f;
    public float jumpHeight = 3;
    public float dashSpeed = 20;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;

    
    [Header("Ground Check")]
    public Transform groundCheck; //player legs
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    
    [Header("Jump Mechanics")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public int maxJumps = 1;

    private int jumpsLeft;
    private float jumpBuffer;
    private float coyoteCounter;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float horizontal;
    private bool isDashing;
    private float dashTime;
    private float dashCooldownTime;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            jumpsLeft = maxJumps;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBuffer = jumpBufferTime;
        }
        else
        {
            jumpBuffer -= Time.deltaTime;
        }
        
        if ((coyoteCounter > 0 || jumpsLeft > 0) && jumpBuffer > 0)
        {
            jumpBuffer = 0;
            
            var jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

            if(!isGrounded)
                jumpsLeft--;
        }

        //dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTime <= 0)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTime = dashCooldown;
        }

        if (isDashing)
        {
            rb.velocity = new Vector2(dashSpeed * horizontal, rb.velocity.y);
            dashTime -= Time.deltaTime;

            if (dashTime <= 0)
            {
                isDashing = false;
            }
        }  
        dashCooldownTime -= Time.deltaTime;      
    }


    private void FixedUpdate()
    {
        if(!isDashing)
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        print(other.relativeVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
