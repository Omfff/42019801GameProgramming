using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSceneManager : MonoBehaviour 
{
    public GameObject[] m_Containers;
    public int m_CurrentIndex = 0;

    private void Awake()
    {
        foreach (GameObject _cnt in m_Containers)
            _cnt.SetActive(false);
        ActivateContainer(m_CurrentIndex);
    }

    public void ActivateContainer(int index)
    {
        if (index >= m_Containers.Length)
            index = 0;
        if (index < 0)
            index = m_Containers.Length - 1;

        if (index >= 0)
        {
            m_Containers[m_CurrentIndex].SetActive(false);
            m_CurrentIndex = index;
            m_Containers[m_CurrentIndex].SetActive(true);
        }
    }

    public void GoToNext()
    {
        ActivateContainer(m_CurrentIndex + 1);
    }

    public void GoToPrevious()
    {
        ActivateContainer(m_CurrentIndex - 1);
    }
}
