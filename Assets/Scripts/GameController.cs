using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static GameController instance;

    [SerializeField] private DissolveEffect dissolveEffect;
    [SerializeField] private OutlineEffect outlineEffect;
    [SerializeField] private MaterialTintColor tintEffect;

    public UI_HotkeyBar uI_HotkeyBar;
    // Start is called before the first frame update
    private static int health = 20;
    private static int maxHealth = 20;
    private static float moveSpeed = 5f;
    private static float bulletSpeed = 14f;
    private static float fireRate = 0.5f;
    private static int xp = 10;
    private static int maxXp = 10;
    private static float buffLastTime = 8.0f;
    private static int shield = 0;
    private static float shieldLastTime = 5.0f;
    private static float shieldCooldown = 10.0f;
    private static float useItemCooldown = 2.0f;
    private static float changeItemCooldown = 0.2f;
    private static float attackCooldown = 2.0f;

    private static float bulletSize = 1.0f;
    public float lastShield;
    public bool isPlayerVisible = true;
    public bool isTimeStop = false;
    public static int currentItems = 0;

    public GameObject bombPrefab;
    public List<GameObject> prefabList = new List<GameObject>();

    public GameObject deathMenu;

    private static int bulletCount = 1;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public static void reInitialize()
    {
        health = 20;
        maxHealth = 20;
        moveSpeed = 5f;
        bulletSpeed = 14f;
        fireRate = 0.5f;
        xp = 10;
        maxXp = 10;
        buffLastTime = 4.0f;
        shield = 0;
        shieldLastTime = 5.0f;
        shieldCooldown = 10.0f;
        useItemCooldown = 2.0f;
        changeItemCooldown = 0.2f;
        attackCooldown = 2.0f;
        bulletSize = 1.0f;

        GameController.instance.isPlayerVisible = true;
        GameController.instance.isTimeStop = false;
        //isPlayerVisible = true;
        //isTimeStop = false;
        currentItems = 0;
    }
    public static int Health { get => health; set => health = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static int Shield { get => shield; set => shield = value; }
    public static float SheildCoolDown { get => shieldCooldown; set => shieldCooldown = value; }
    public static float UseItemCooldown { get => useItemCooldown; set => useItemCooldown = value; }
    public static float ChangeItemCooldown { get => changeItemCooldown; set => changeItemCooldown = value; }
    public static float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }
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
            GameController.instance.outlineEffect.StopOutline();
            GameController.instance.tintEffect.SetTintColor(new Color(1, 0, 0, 0.7f));
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
        if (healAmount != 0)
        {
            GameController.instance.tintEffect.SetTintColor(new Color(0, 1, 0, 0.7f));
            health = Mathf.Min(maxHealth, health + healAmount);
        }
    }
    public static void AddXp(int xpAmount)
    {
        GameController.instance.tintEffect.SetTintColor(new Color(0, 0, 1, 0.7f));
        xp = Mathf.Min(maxXp, xp + xpAmount);
    }
    public static void Buff(float moveSpeedChange, float attackSpeedChange, float bulletSizeChange, int bulletCountChange, float bulletSpeedChange)
    {
        MoveSpeedChange(moveSpeedChange);
        FireRateChange(attackSpeedChange);
        BulletSizeChange(bulletSizeChange);
        BulletCountChange(bulletCountChange);
        BulletSpeedChange(bulletSpeedChange);
        GameController.instance.outlineEffect.StartOutline();
        GameController.instance.StartCoroutine(
            GameController.instance.Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(0.7f);
        //MoveSpeedChange(-moveSpeedChange);
        //FireRateChange(-attackSpeedChange);
        //BulletSizeChange(-bulletSizeChange);
        //BulletCountChange(-bulletCountChange);
        //BulletSpeedChange(-bulletSpeedChange);
        GameController.instance.outlineEffect.StopOutline();
    }

    public static void MoveSpeedChange(float speed)
    {
        if (speed > 0)
        {
            GameController.instance.outlineEffect.SetOutlineColor(Color.blue);
        }
        moveSpeed += speed;
    }

    public static void BulletSpeedChange(float speed)
    {
        if (speed > 0)
        {
            GameController.instance.outlineEffect.SetOutlineColor(Color.red);
        }
        bulletSpeed += speed;
    }

    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
        if (rate > 0)
            GameController.instance.outlineEffect.SetOutlineColor(Color.red);

    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
        if (size > 0)
            GameController.instance.outlineEffect.SetOutlineColor(Color.red);

    }

    public static void BulletCountChange(int size)
    {
        bulletCount += size;
        if (size > 0)
            GameController.instance.outlineEffect.SetOutlineColor(Color.yellow);

    }

    public static void AddShield()
    {
        if (Time.time > GameController.instance.lastShield + SheildCoolDown)
        {
            GameController.instance.uI_HotkeyBar.ShieldCooldown();
            shield += 4;
            GameController.instance.lastShield = Time.time;
            GameController.instance.outlineEffect.SetOutlineColor(Color.white);
            GameController.instance.outlineEffect.StartOutline();
            Debug.Log("Current Shield = " + shield);
            GameController.instance.StartCoroutine(GameController.instance.ShieldLast());
        }


    }

    IEnumerator ShieldLast()
    {
        yield return new WaitForSeconds(shieldLastTime);
        if (shield != 0)
        {
            shield = 0;
            GameController.instance.outlineEffect.StopOutline();
        }
    }

    public static void AddItems(Item item)
    {
        GameController.instance.storageItems.Add(item);
        currentItems = (currentItems + 1) % GameController.instance.storageItems.Count;
        GameController.instance.uI_HotkeyBar.ItemChanged();
    }
    
    public static void ChangeItems()
    {
        if (GameController.instance.storageItems.Count > 0)
        {
            currentItems = (currentItems + 1) % GameController.instance.storageItems.Count;
            GameController.instance.uI_HotkeyBar.ItemChanged();
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
                    GameController.instance.dissolveEffect.StartDissolve(0.7f);
                    GameController.instance.StartCoroutine(GameController.instance.VisibleOut());
                    GameController.instance.storageItems.RemoveAt(currentItems);
                    //特效
                    break;
                case "count":
                    BulletCountChange(2);
                    GameController.instance.outlineEffect.SetOutlineColor(Color.yellow);
                    GameController.instance.StartCoroutine(GameController.instance.StrengthenEnd());
                    GameController.instance.storageItems.RemoveAt(currentItems);
                    //BulletSpeedChange(-bulletSpeedChange);
                    break;
                case "bomb":
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    GameController.Instantiate(GameController.instance.bombPrefab, player.transform.position, player.transform.rotation);
                    
                    GameController.instance.storageItems.RemoveAt(currentItems);
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
            GameController.instance.uI_HotkeyBar.ItemChanged();
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
        GameController.instance.dissolveEffect.StopDissolve(0.7f);
        GameController.instance.isPlayerVisible = true;
    }

    IEnumerator StrengthenEnd()
    {
        yield return new WaitForSeconds(5f);
        GameController.instance.outlineEffect.StopOutline();
        BulletCountChange(-2);
    }

    IEnumerator XpRecovery()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.0f);
            xp += 1;
            xp = xp > 10 ? 10 : xp;
        }
    }

    public static void KillPlayer()
    {
        GameController.instance.deathMenu.SetActive(true);
    }


    void Start()
    {
        StartCoroutine(XpRecovery());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
