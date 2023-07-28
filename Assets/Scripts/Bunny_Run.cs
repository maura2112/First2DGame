using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny_Run : MonoBehaviour
{
    private Vector3 start;

    private float currentDistance;
    [SerializeField]private float speed =4f;
    [SerializeField]private float distance = 3f;

    private int reached;
     
    //private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        start = transform.localPosition;
        reached = 1;
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right * reached * speed * Time.deltaTime);

        currentDistance += Mathf.Abs(speed * Time.deltaTime);

        if (currentDistance >= distance)
        {
            Flip();
            reached *= -1;
            currentDistance = 0f;
        }

    }

    void Flip()
    {

        Vector2 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}
