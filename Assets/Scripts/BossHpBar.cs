using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossHpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject boss;
    public Room bossRoom;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void Start()
    {
        boss = GameObject.Find("BOSS");

        SetMaxHealth(boss.GetComponent<Boss>().health);
        //SceneManager.GetSceneByName("Basement8");
        //gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<CanvasGroup>().alpha = 0;

    }
    public void Update()
    {
        //Debug.Log(CameraController.instance.currRoom.name);
        //Debug.Log(SceneManager.GetActiveScene().name);
        //Debug.Log(SceneManager.GetSceneByName("Basement8").GetRootGameObjects());
        if (bossRoom == null)
        {
            Debug.Log(RoomController.instance.loadedRooms);
            bossRoom = RoomController.instance.loadedRooms.Find(delegate (Room room)
            {
                return room.name.Contains("Basement-8");
            });
        }
        if (CameraController.instance.currRoom == bossRoom)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
        else
        {
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
        }
        if (boss == null)
        {
            SetHealth(0);
        }
        else
        {
            SetHealth(boss.GetComponent<Boss>().health);
        }
        //for test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameController.DamagePlayer(1);
        }
    }
}
