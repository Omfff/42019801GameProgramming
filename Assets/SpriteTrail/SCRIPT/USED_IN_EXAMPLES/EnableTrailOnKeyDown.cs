using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteTrail;

public class EnableTrailOnKeyDown : MonoBehaviour 
{
    public KeyCode m_EnableKey = KeyCode.Space;
    public KeyCode m_DisableKey = KeyCode.Escape;
    public SpriteTrail.SpriteTrail m_Trail;


    private void Update()
    {
        if(Input.GetKeyDown(m_EnableKey))
        {
            m_Trail.EnableTrail();
        }
        if(Input.GetKeyDown(m_DisableKey))
        {
            m_Trail.DisableTrail();
        }
    }
}
