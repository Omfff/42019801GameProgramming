using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void Start()
    {
        SetMaxHealth(GameController.MaxXp);
    }
    public void Update()
    {
        SetHealth(GameController.Xp);
        //for test
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    GameController.RemoveXp(1);
        //}
    }
}
