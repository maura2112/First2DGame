using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player Behavior
    //
    float x, y;
    private float maxYVelocity;
    //Run
    private float moveVertical;
    private float horizontal;
    private float speed = 8f;
    public bool isFacingRight = true;
    public bool isGrounded;
    [SerializeField] private ParticleSystem dust;

    //Jump
    public bool isJumping;
    private float jumpingPower = 30f;
    private bool doubleJump;

    //Dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    //Game Object priority
    private AudioSource jumpSE;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private PlayerHealth playerHealth;
    private Animator animator;




    //Ground, Idle, Wall
    public bool isWallSliding;
    //private  bool isWall = false;
    private float wallSlidingSpeed = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    public bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(20f, 40f);
    public Ball ball;
    




    void Start()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = rb.velocity.y;
        animator = gameObject.GetComponent<Animator>();
        jumpSE = gameObject.GetComponent<AudioSource>();
        
        

    }
    private void Update()
    {


        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        x = Input.GetAxisRaw("Horizontal");
        y = rb.velocity.y;
        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        animator.SetFloat("Crouch", moveVertical);


        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            maxYVelocity = 0;
            CreateDust();

            if (IsGrounded() || doubleJump)
            {
                jumpSE.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                doubleJump = !doubleJump;
            }
        }
        
            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                animator.SetFloat("Jump", 1f);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        WallSlide();
        WallJump();
        if (!isWallJumping)
        {
            Flip();
        }

    }

    private void FixedUpdate()
    {

        if (isDashing)
        {
            return;
        }
        
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        


        Debug.DrawLine(transform.localPosition, transform.localPosition + new Vector3(rb.velocity.x, rb.velocity.y, 0));


        if (isGrounded)
        {
            if (maxYVelocity <= -18)
            {
                playerHealth.FallDamage();
                maxYVelocity = 0;
            }
        }
        else if (isJumping)
        {
            if (rb.velocity.y < maxYVelocity)
            {
                maxYVelocity = rb.velocity.y;
            }
        }

        if (moveVertical < 0.1f && !isJumping)
        {
            animator.SetFloat("Jump", 0);
            rb.AddForce(new Vector2(0f, moveVertical * jumpingPower * Time.deltaTime), ForceMode2D.Impulse);
        }

    }


    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                //Vector3 localScale = transform.localScale;
                //localScale.x *= -1;
                //transform.localScale = localScale;
                transform.Rotate(0, 180, 0);

            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            CreateDust();
            isFacingRight = !isFacingRight;
            //Vector3 localScale = transform.localScale;
            //localScale.x *= -1f;
            //transform.localScale = localScale;
            transform.Rotate(0, 180, 0);
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }



    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (isFacingRight)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else
        {
            rb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        }

        tr.emitting = true;
        transform.Find("dash_Audio").GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void CreateDust()
    {
        dust.Play();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "OneWayPlatform")
        {
            transform.parent = collision.transform;
            isJumping = false;
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            animator.Play("Hurt_Animation");
        }
    }




    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "OneWayPlatform")
        {
            transform.parent = null;
            isJumping = true;
            isGrounded = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            animator.Play("Hurt_Animation");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }



}
