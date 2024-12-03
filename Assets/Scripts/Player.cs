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
    
    [Header("Ground Check")]
    public Transform groundCheck; //player legs
    public float groundCheckRadius = 0.2f;
    public LayerMask groundMask;
    
    [Header("Jump Mechanics")]
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    private float jumpBuffer;
    private float coyoteCounter;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float horizontal;
    
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
        
        if (coyoteCounter > 0 && jumpBuffer > 0)
        {
            jumpBuffer = 0;
            
            var jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
    }


    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
