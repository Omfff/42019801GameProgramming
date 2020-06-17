using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCharacterControler2D : MonoBehaviour 
{
    Rigidbody2D m_RigidBody;
    public float m_Force;
    public float m_MaxVelocityToBoost;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
		m_RigidBody.velocity = new Vector2(5,5);

	}


    private void Update()
    {
        if (m_RigidBody.velocity.x <= m_MaxVelocityToBoost && Input.GetKey(KeyCode.RightArrow))
            m_RigidBody.AddForce(new Vector2(m_Force, 0), ForceMode2D.Force);
        if (m_RigidBody.velocity.x >= -m_MaxVelocityToBoost && Input.GetKey(KeyCode.LeftArrow))
            m_RigidBody.AddForce(new Vector2(-m_Force, 0), ForceMode2D.Force);
        if (m_RigidBody.velocity.y <= m_MaxVelocityToBoost && Input.GetKey(KeyCode.UpArrow))
            m_RigidBody.AddForce(new Vector2(0, m_Force), ForceMode2D.Force);
        if (m_RigidBody.velocity.y >= -m_MaxVelocityToBoost && Input.GetKey(KeyCode.DownArrow))
            m_RigidBody.AddForce(new Vector2(0, -m_Force), ForceMode2D.Force);
    }
}
