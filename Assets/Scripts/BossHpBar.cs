using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public GameObject boss;

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
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (CameraController.instance.currRoom.name == "Basement8")
        {
            gameObject.SetActive(true);
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
