using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        PlayerPrefs.SetString("Level", "Basement");
        SceneManager.LoadScene("AlfheimLoading");
    }

    public void onContinueClick()
    {
        var currentWorldName = PlayerPrefs.GetString("Level");
        switch (currentWorldName)
        {
            case "Basement":
                SceneManager.LoadScene("AlfheimLoading");
                break;
            case "Forest":
                SceneManager.LoadScene("MidgardLoading");
                break;
            case "Hell":
                SceneManager.LoadScene("HelheimLoading");
                break;
        }
    }
}
