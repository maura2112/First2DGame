using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadEnemy : MonoBehaviour
{
    private Animator animator;
    private AudioSource headEnemy;
    private float bounce = 15f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        headEnemy = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision duoi chan player la trigger collider
        if (collision.tag == "Player")
        {
            headEnemy.Play();
            animator.Play("Enemt-death-Animation");
            
            Invoke("OnDisable", 0.2f);
            
        }
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
