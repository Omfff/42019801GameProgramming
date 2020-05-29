using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float speed;

    public static void Create(Transform pfBomb, Vector3 spawnPosition, Vector3 targetPosition, Action<Vector3> onExplodeAction)
    {
        BombController bomb = Instantiate(pfBomb, spawnPosition, Quaternion.identity).GetComponent<BombController>();
    }

    private Action<Vector3> onExplodeAction;

    private void Setup(Vector3 targetPosition, Action<Vector3> onExplodeAction)
    {
        this.onExplodeAction = onExplodeAction;
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float moveSpeed = speed;// Mathf.Clamp(distance * 4f, 50f, 250f);
        gameObject.GetComponent<Rigidbody2D>().velocity = moveDirection * moveSpeed;
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = -100f;
    }

    private void bombDestory()
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<CircleCollider2D>());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
