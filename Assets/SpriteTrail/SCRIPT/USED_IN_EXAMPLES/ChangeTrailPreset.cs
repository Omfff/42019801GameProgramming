using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SpriteTrail;

public class ChangeTrailPreset : MonoBehaviour 
{
	public SpriteTrail.SpriteTrail m_Trail;
	public TrailPreset[] m_Presets;
	public int m_CurrentPresetIndex = 0;
	int m_PreviousPresetIndex = -1;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			NextPreset();
		}

		if(m_CurrentPresetIndex != m_PreviousPresetIndex)
		{
			if (m_CurrentPresetIndex < 0)
				m_CurrentPresetIndex = m_Presets.Length - 1;
			if (m_CurrentPresetIndex >= m_Presets.Length)
				m_CurrentPresetIndex = 0;

			if(m_CurrentPresetIndex >= 0)
			{
				m_Trail.DisableTrail();
				m_Trail.SetTrailPreset(m_Presets[m_CurrentPresetIndex]);
				m_Trail.EnableTrail();
				m_PreviousPresetIndex = m_CurrentPresetIndex;
			}
		}
	}

	public void NextPreset()
	{
		m_CurrentPresetIndex++;
	}

	public void PreviousPreset()
	{
		m_CurrentPresetIndex--;
	}

	public void SetPresetIndex(int index)
	{
		m_CurrentPresetIndex = index;
	}

}
