using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth = 5;

    private Animator animator;
    private AudioSource audioSource;

    [SerializeField] private PlayerController playerController;
    bool onGrounded;
    float maxYVelocity;
    private Rigidbody2D rb;
    [SerializeField] float beingFall;

    [SerializeField] private float fallBounce;



    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        //audioSource = gameObject.GetComponent<AudioSource>()[1];
        audioSource = transform.Find("hurt_Audio").GetComponent<AudioSource>();
        health = maxHealth;
        rb = playerController.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //onGrounded = playerController.IsGrounded();
        //if (onGrounded) {
        //    maxYVelocity = rb.velocity.y;
        //    Debug.Log("Velocity when on the ground: "+rb.velocity.y);
        //}
        //else
        //{
        //    Debug.Log("Velocity when falling: " + rb.velocity.y);
        //    if (maxYVelocity > beingFall)
        //    {

        //        FallDamage();
        //        maxYVelocity = 0;
        //    }
        //}
    }
    public void TakeDamage(float amount)
    {
        audioSource.Play();
        ShakeCamera.Instance.Shake(5f, 0.1f);
        playerController.GetComponent<Animator>().Play("Hurt_Animation");
        health -= amount;
        if (health <= 0)
        {
            
            animator.Play("Enemt-death-Animation");
            Invoke("OnDisable", .3f);
        }

    }

    public void FallDamage()
    {
        audioSource.Play();
        ShakeCamera.Instance.Shake(10f, 0.2f);
        playerController.GetComponent<Rigidbody2D>().AddForce(Vector2.up * fallBounce, ForceMode2D.Impulse);
        playerController.GetComponent<Animator>().Play("Hurt_Animation");
        health -= 5;
        if (health <= 0)
        {

            animator.Play("Enemt-death-Animation");
            Invoke("OnDisable", .3f);
        }
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }


}
