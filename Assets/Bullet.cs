using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;

    private void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyDamage enemy = collision.GetComponent<EnemyDamage>();
        if (enemy != null)
        {
            enemy.Die();

        }
        Destroy(gameObject);
    }

}


