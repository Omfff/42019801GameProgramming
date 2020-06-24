using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour 
{
    public Vector2 m_Speed;


    private void Update()
    {
        transform.position = (Vector2) transform.position + (m_Speed * Time.deltaTime);
    }
}
