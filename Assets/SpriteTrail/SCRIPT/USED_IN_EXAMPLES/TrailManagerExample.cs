using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpriteTrail;

public class TrailManagerExample : MonoBehaviour 
{
    public SpriteTrail.SpriteTrail[] m_Trails;
    public int m_CurrentTrailIndex = 0;
    public GameObject m_Character;
    public GameObject m_UI;

    void OnDisable()
    {
        if(m_UI != null)
            m_UI.SetActive(false);
    }

    private void OnEnable()
    {
        m_Character.transform.position = Vector2.zero;
        m_Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_Character.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_Character.GetComponent<SimpleCharacterControler2D>().m_Force, 0), ForceMode2D.Impulse);
        m_UI.SetActive(true);
        foreach (SpriteTrail.SpriteTrail _trail in m_Trails)
            _trail.DisableTrail();
        ActivateTrail(m_CurrentTrailIndex);
    }

    public void ActivateTrail(int index)
    {
        if (index >= m_Trails.Length)
            index = 0;
        if (index < 0)
            index = m_Trails.Length - 1;

        if(index >= 0)
        {
            m_Trails[m_CurrentTrailIndex].DisableTrail();
            m_CurrentTrailIndex = index;
            m_Trails[m_CurrentTrailIndex].EnableTrail();
        }
    }

    public void GoToNext()
    {
        ActivateTrail(m_CurrentTrailIndex + 1);
    }

    public void GoToPrevious()
    {
        ActivateTrail(m_CurrentTrailIndex - 1);
    }
}
