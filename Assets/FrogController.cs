using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : MonoBehaviour
{
    public bool facingRight = false;
    public LayerMask Platform;

    public bool isGrounded = false;
    public bool isFalling = false;
    public bool isJumping = false;

    public float jumpForceX = 2f;
    public float jumpForceY = 4f;

    public float lastYPos = 0;

    public enum Animations
    {
        Idle = 0,
        Jumping = 1,
        Falling = 2
    };

    public Animations currentAnimations;

    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public Animator anim;

    public float idleTime = 2f;
    public float currentIdleTime = 0f;
    public bool isIdle = true;


    // Start is called before the first frame update
    void Start()
    {
        lastYPos = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (isIdle)
        {
            currentIdleTime += Time.deltaTime;
            if (currentIdleTime >= idleTime)
            {

                currentIdleTime = 0f;
                facingRight = !facingRight;
                sp.flipX = facingRight;
                Jump();
            }

        }
        isGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
                                            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.49f),
                                            Platform);
        if (isGrounded && isFalling)
        {
            isFalling = false;
            isJumping = false;
            isIdle = true;
            ChangAnimation(Animations.Idle);
        }
        else if (transform.position.y > lastYPos && !isGrounded && !isIdle)
        {
            isJumping = true;
            isFalling = false;
            ChangAnimation(Animations.Jumping);
        }
        else if (transform.position.y < lastYPos && !isGrounded && !isIdle)
        {
            isJumping = false;
            isFalling = true;
            ChangAnimation(Animations.Falling);
        }
        lastYPos = transform.position.y;
    }
    void Jump()
    {
        isJumping = true;
        isIdle = false;
        int diretion = 0;
        if (facingRight == true)
        {
            diretion = 1;

        }
        else
        {
            diretion = -1;
        }
        rb.velocity = new Vector2(jumpForceX * diretion, jumpForceY);
        Debug.Log("Jump!");

    }

    void ChangAnimation(Animations newAnim)
    {
        if (currentAnimations != newAnim)
        {
            currentAnimations = newAnim;
            anim.SetInteger("state", (int)newAnim);
        }
    }
}
