using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    [SerializeField] protected float damage;
    protected float bounce = 15f;
    protected Animator animator;
    protected AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        audio = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Die()
    {
        audio.Play();
        animator.Play("Enemt-death-Animation");
        gameObject.GetComponent<Collider2D>().enabled = false;
        
        Invoke("OnDisable", .2f);
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2f, 2f) * bounce, ForceMode2D.Impulse);
            playerHealth.TakeDamage(damage);
        }

    }
    

}
