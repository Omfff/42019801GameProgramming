using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour 
{
	public void GoToScene(string name)
	{
		SceneManager.LoadScene(name);
	}
}
