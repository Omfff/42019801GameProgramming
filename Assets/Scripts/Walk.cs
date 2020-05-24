using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    const float minSpeed = 0.0001f;
    Animator animator;
    float xSpeed;
    float ySpeed;
    float speedFactor;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        xSpeed = 0f;
        ySpeed = 0f;
        speedFactor = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        {
            xSpeed = Input.GetAxis("Horizontal");
            ySpeed = Input.GetAxis("Vertical");
            animator.SetFloat("xSpeed", xSpeed);
            animator.SetFloat("ySpeed", ySpeed);
        }
        Movement();
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speedFactor * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speedFactor * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * speedFactor * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * speedFactor * Time.deltaTime);
            transform.eulerAngles = new Vector2(0, 0);
        }
    }
}
