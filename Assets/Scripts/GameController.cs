using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    // Start is called before the first frame update
    private static int health = 10;
    private static int maxHealth = 10;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.5f;
    private static int xp = 10;
    private static int maxXp = 10;
    private static float buffLastTime = 4.0f;
    private static int shield = 0;
    private static float shieldLastTime = 5.0f;
    private static float shieldCooldown = 10.0f;
    private static float useItemCooldown = 2.0f;
    private static float changeItemCooldown = 2.0f;

    private static float bulletSize = 1.0f;
    private float lastShield;
    public bool isPlayerVisible = true;
    public bool isTimeStop = false;
    public static int currentItems = 0;

    private static int bulletCount = 3;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public static int Health { get => health; set => health = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static int Shield { get => shield; set => shield = value; }
    public static float SheildCoolDown { get => shieldCooldown; set => shieldCooldown = value; }
    public static float UseItemCooldown { get => useItemCooldown; set => useItemCooldown = value; }
    public static float ChangeItemCooldown { get => changeItemCooldown; set => changeItemCooldown = value; }
    public static int Xp { get => xp; set => xp = value; }
    public static int MaxXp { get => maxXp; set => maxXp = value; }

    public static float BulletSize { get => bulletSize; set => bulletSize = value; }
    public static int BulletCount { get => bulletCount; set => bulletCount = value; }
    public static int CurrentItems { get => currentItems; set => currentItems = value; }

    public List<Item> storageItems = new List<Item>();

    public static void DamagePlayer(int damage)
    {
        if (shield > damage)
        {
            shield -= damage;
        }
        else
        {
            shield = 0;
            health -= damage - shield;
            health = health < 0 ? 0 : health;
        }


        if (Health <= 0)
        {
            KillPlayer();
        }
    }
    public static void RemoveXp(int damage)
    {
        xp -= damage;
    }
    public static void HealPlayer(int healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }
    public static void AddXp(int xpAmount)
    {
        xp = Mathf.Min(maxXp, xp + xpAmount);
    }
    public static void Buff(float moveSpeedChange, float attackSpeedChange, float bulletSizeChange, int bulletCountChange)
    {
        MoveSpeedChange(moveSpeedChange);
        FireRateChange(attackSpeedChange);
        BulletSizeChange(bulletSizeChange);
        BulletCountChange(bulletCountChange);

        GameController.instance.StartCoroutine(
            GameController.instance.Reset(moveSpeedChange, attackSpeedChange, bulletSizeChange, bulletCountChange));
    }

    IEnumerator Reset(float moveSpeedChange, float attackSpeedChange, float bulletSizeChange, int bulletCountChange)
    {
        yield return new WaitForSeconds(buffLastTime);
        MoveSpeedChange(-moveSpeedChange);
        FireRateChange(-attackSpeedChange);
        BulletSizeChange(-bulletSizeChange);
        BulletCountChange(-bulletCountChange);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }

    public static void BulletCountChange(float size)
    {
        bulletSize += size;
    }

    public static void AddShield()
    {
        if (Time.time > GameController.instance.lastShield + SheildCoolDown)
        {
            shield += 4;

            GameController.instance.StartCoroutine(GameController.instance.ShieldLast());
        }


    }

    IEnumerator ShieldLast()
    {
        yield return new WaitForSeconds(shieldLastTime);
        if (shield != 0)
        {
            shield = 0;

        }
    }

    public static void AddItems(Item item)
    {
        GameController.instance.storageItems.Add(item);
        currentItems = (currentItems + 1) % GameController.instance.storageItems.Count;
    }
    
    public static void ChangeItems()
    {
        if (GameController.instance.storageItems.Count > 0)
        {
            currentItems = (currentItems + 1) % GameController.instance.storageItems.Count;
        }
    }

    public static void UseItems()
    {
        if (GameController.instance.storageItems.Count > 0)
        {
            Item item = GameController.instance.storageItems[currentItems];
            switch (item.name)
            {
                case "time":
                    GameController.instance.isTimeStop = true;
                    GameController.instance.StartCoroutine(GameController.instance.TimeOut());
                    GameController.instance.storageItems.RemoveAt(currentItems);

                    break;
                case "stealth":
                    GameController.instance.isPlayerVisible = false;
                    GameController.instance.StartCoroutine(GameController.instance.VisibleOut());
                    GameController.instance.storageItems.RemoveAt(currentItems);

                    //特效
                    break;
            }
            if (GameController.instance.storageItems.Count > 0)
            {
                currentItems = (currentItems - 1) % GameController.instance.storageItems.Count;
            }
            else
            {
                currentItems = 0;
            }
        }
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(buffLastTime);
        GameController.instance.isTimeStop = false;
    }

    IEnumerator VisibleOut()
    {
        yield return new WaitForSeconds(buffLastTime);
        GameController.instance.isPlayerVisible = true;
    }

    public static void KillPlayer()
    {

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
