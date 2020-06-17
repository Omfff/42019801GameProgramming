using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteTrail;

public class BasicPath : MonoBehaviour 
{
    public bool m_Lerp = false;
    public float m_CurSpeed = 1f;
    public Transform[] m_Waypoints;
    int m_CurWayPointIndex = 0;
    float m_ClampFloor = .1f;
    public SpriteTrail.SpriteTrail m_Trail;
    public bool m_FlipX = true;
	public bool FlipXOnEnd = true;
	public Transform m_ItemToMove;

    private void Update()
    {
        if(m_Lerp)
        {
			m_ItemToMove.position = Vector2.Lerp(m_ItemToMove.position, m_Waypoints[m_CurWayPointIndex].position, m_CurSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 _Dir = ((m_Waypoints[m_CurWayPointIndex].position - m_ItemToMove.position).normalized);
			m_ItemToMove.position += _Dir * m_CurSpeed * Time.deltaTime;
        }

        float _TmpDist = Vector2.Distance(m_ItemToMove.position, m_Waypoints[m_CurWayPointIndex].position);
        if(_TmpDist <= m_ClampFloor)    //we reached the waypoint : go to next
        {

            if(m_Trail != null)
            {
                //m_Trail.EnableTrailEffect(true);    //Here i force the creation of a new element (usefull at the begining of a fast movement, so the first sprite is created right at the begining of the move)
				if(FlipXOnEnd)
					m_Trail.m_SpriteToDuplicate.flipX = !m_Trail.m_SpriteToDuplicate.flipX;
            }
            m_CurWayPointIndex++;
            if (m_CurWayPointIndex >= m_Waypoints.Length)
            {
                m_CurWayPointIndex = 0;
            }
        }
    }
}
