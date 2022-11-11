using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public GameObject[] flipObjects;
    public float moveInput;
    public float speed;
    [Space]
    public float jumpForce;
    [Space]
    public float fallMulti = 2.5f;
    public float lowJumpMulti = 2f;
    [Space]
    public float coyoteTime = .2f;
    float coyoteTimer;
    float jumpBufferTimer;
    public float jumpBufferTime = 0.1f;
    [Space(5)]
    public bool isGrounded;
    public float checkRange;
    public Transform groundCheck;
    public LayerMask groundMask;

    Rigidbody2D rb;
    bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        //Inputs
        moveInput = Input.GetAxis("Horizontal");

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRange, groundMask);

        if(isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        //Jump Buffer!
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        //if(Input.GetKeyDown(KeyCode.W) && isGrounded || Input.GetKeyDown(KeyCode.Space) && isGrounded)
        //Replacing the isGrounded bool with coyoteTimer checkes, makes sure the coyotetimer is still availbile to jump! or smth
        if(Input.GetKeyDown(KeyCode.Space) && coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpBufferTimer = 0f;
        }

        //Resets Coyote Timer to remove chances of doubleJumping outter nowhere XD
        if(Input.GetKeyUp(KeyCode.Space))
        {
            coyoteTimer = 0f;
        }

        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMulti - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMulti - 1) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if(!facingRight && moveInput > 0)
        {
            Flip();
        }
        else if(facingRight && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        
        //FlipObjects[opposite of the direction]
        for (int i = 0; i < flipObjects.Length; i++)
        {
            Vector3 scaler_ = flipObjects[i].transform.localScale;
            scaler_.x *= -1;
            flipObjects[i].transform.localScale = scaler_;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, checkRange);
    }
}
