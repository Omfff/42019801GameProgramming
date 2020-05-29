using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float lifeTimer;

    private bool isDestroyMe = false;

    private void Start()
    {
        if (lifeTimer > 0.01f)
        {
            Destroy(gameObject, lifeTimer);
        }
    }

    void Update()
    {
        if (isDestroyMe)
        {
            Debug.Log("line destroy");
            Destroy(gameObject);
        }
    }

    public void DestroyMe()
    {
        isDestroyMe = true;
    }
}
