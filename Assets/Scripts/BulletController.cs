﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
public enum BulletType
{
    Normal,

    Bomb,

    Slalom,

    Tracking,

    Projectile,
};

public class BulletController : MonoBehaviour
{
    public float lifeTime;

    public bool isEnemyBullet = false;

    private Vector2 lastPos;

    private Vector2 curPos;

    private Vector2 playerPos;

    public Vector2 destPos;

    public float bulletSpeed;

    public float damage;

    private Vector2 moveDir;

    private bool isBombing = false;

    public GameObject explosionEffect;

    public BulletType bulletType;

    public GameObject Player;

    public Vector2 startPos;

    public bool slalomFirst;

    public float acceleration = 10.0f;

    public bool projectileStart;
    // Start is called before the first frame update
    void Start() 
    {
        //StartCoroutine(DeathDelay());
        switch (bulletType)
        {
            case BulletType.Normal:
                bulletSpeed = 5f;
                StartCoroutine(DeathDelay());
                break;
            case BulletType.Bomb:
                bulletSpeed = 2f;
                break;
            case BulletType.Slalom:
                bulletSpeed = 0f;
                startPos = transform.position;
                slalomFirst = true;
                break;
            case BulletType.Tracking:
                bulletSpeed = 4f;
                StartCoroutine(DeathDelay());
                break;
            case BulletType.Projectile:
                bulletSpeed = 4f;
                projectileStart = true;
                //Physics2D.IgnoreCollision();
                break;
        }
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        if(isEnemyBullet)
        {
            //curPos = transform.position;
            //transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            //if(curPos == lastPos)
            //{
            //    Destroy(gameObject);
            //}
            //lastPos = curPos;
            switch (bulletType)
            {
                case BulletType.Normal:
                    curPos = transform.position;
                    //// how to set this speed to the bullet in Enemy Controller
                    transform.position += new Vector3(moveDir.x * bulletSpeed * Time.deltaTime, moveDir.y * bulletSpeed * Time.deltaTime, 0);//5f
                    if (curPos == lastPos)
                    {
                        Destroy(gameObject);
                    }
                    lastPos = curPos;
                    break;
                case BulletType.Bomb:
                    curPos = transform.position;
                    // how to set this speed to the bullet in Enemy Controller
                    transform.position = Vector2.MoveTowards(transform.position, playerPos, bulletSpeed * Time.deltaTime);//5f
                    if (curPos == lastPos && !isBombing)
                    {
                        StartCoroutine(BombCountdownToStart());
                    }
                    lastPos = curPos;
                    break;
                case BulletType.Slalom:
                    curPos = transform.position;
                    destPos = playerPos + moveDir * 2.0f;
                    //Debug.Log("1:         " + (curPos - destPos).sqrMagnitude);
                    if ((curPos - destPos).sqrMagnitude < (startPos - destPos).sqrMagnitude / 4)
                    {
                        acceleration = -10;
                    }
                    else
                    {
                        acceleration = 10;
                    }
                    bulletSpeed += acceleration * Time.deltaTime;
                    if (slalomFirst)
                    {
                        transform.position += new Vector3(moveDir.x * bulletSpeed * Time.deltaTime, moveDir.y * bulletSpeed * Time.deltaTime, 0);
                        //transform.position = Vector2.MoveTowards(transform.position, destPos, bulletSpeed * Time.deltaTime);
                    }
                    else
                    {
                        Debug.Log("back");
                        transform.position += new Vector3(moveDir.x * bulletSpeed * Time.deltaTime, moveDir.y * bulletSpeed * Time.deltaTime, 0);
                        //transform.position = Vector2.MoveTowards(transform.position, startPos, bulletSpeed * Time.deltaTime);

                    }
                    if (Vector2.Dot((destPos - curPos),(destPos - startPos)) < 0)
                    {
                        StartCoroutine(SlalomDelay());
                    }
                    if ((curPos - startPos).sqrMagnitude < 0.5 && !slalomFirst)
                    {
                        Destroy(gameObject);
                    }
                    break;

                case BulletType.Tracking:
                    // need to get player position
                    curPos = transform.position;
                    transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, bulletSpeed * Time.deltaTime);
                    
                    // can refine the tracking routine
                    break;

                case BulletType.Projectile:
                    //transform.position += new Vector3(moveDir.x * bulletSpeed * Time.deltaTime, moveDir.y * bulletSpeed * Time.deltaTime, 0);//5f
                    if (projectileStart)
                    {
                        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(moveDir.x * bulletSpeed, moveDir.y * bulletSpeed, 0);
                        
                        projectileStart = false;
                    }
                    if (GetComponent<Rigidbody2D>().velocity.sqrMagnitude < 1f)
                    {
                        Destroy(gameObject);
                    }

                    transform.localScale = new Vector3(4*GetComponent<Rigidbody2D>().velocity.magnitude/bulletSpeed, 4*GetComponent<Rigidbody2D>().velocity.magnitude/bulletSpeed, 1);
                    break;
            }
        }
    }

    // only used in tracking
    public void SetPlayer(GameObject player)
    {
        Player = player;
    }


    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
        moveDir = new Vector2(playerPos.x - transform.position.x, playerPos.y - transform.position.y);
        moveDir = moveDir.normalized;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    IEnumerator BombCountdownToStart()
    {
        isBombing = true;
        while (lifeTime > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.02f, transform.localScale.y + 0.02f, 1);
            transform.GetComponent<BombCircle>().SetSelectedVisible(true);
            yield return new WaitForSeconds(0.1f);
            lifeTime -= 0.1f;
            //Debug.Log("life time" + lifeTime);
            //yield return null;
        }

        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, gameObject.GetComponent<Renderer>().bounds.size.x);
        Collider2D collider;
        for (int i = 0; i < colliders.Length; i++)
        {
            collider = colliders[i];
            if (collider.transform.tag == "Player")
            {
                GameController.DamagePlayer(2);
            }
        }

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }

    IEnumerator SlalomDelay()
    {
        yield return null;
        slalomFirst = false;
        bulletSpeed = 0;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            switch (gameObject.name)
            {
                case "thunderball(Clone)":
                    //麻痹
                    enemy.FrozenMovement();
                    enemy.StartCoroutine(ResetThunder(enemy));
                    enemy.getHurt(damage);
                    break;
                case "Waterball(Clone)":
                    //减速减攻速
                    float coolDown = enemy.GetCoolDownTime();
                    enemy.SlowDownMovingSpeed(0.5f);//slow down to 10% of origin speed
                    enemy.SetCoolDownTime(coolDown + 2);
                    enemy.StartCoroutine(ResetWater(enemy, coolDown));//coolDown
                    enemy.getHurt(damage);
                    break;
                case "Fireball(Clone)":
                    //持续伤害
                    enemy.StartCoroutine(ContinuousDamage(enemy));
                    break;
                default:
                    enemy.getHurt(damage);
                    break;
            }
            //col.gameObject.GetComponent<Enemy>().getHurt(damage);
            Destroy(gameObject);
        }
        else if (col.tag == "Player" && isEnemyBullet)
        {
            switch (bulletType)
            {
                case BulletType.Normal:
                    GameController.DamagePlayer(1);
                    Destroy(gameObject);
                    break;
                case BulletType.Tracking:
                    GameController.DamagePlayer(1);
                    Destroy(gameObject);
                    break;
                case BulletType.Projectile:
                    GameController.DamagePlayer(1);
                    Destroy(gameObject);
                    break;
                case BulletType.Slalom:
                    GameController.DamagePlayer(1);
                    if (!slalomFirst)
                    {
                        Destroy(gameObject);
                    }
                    break;
                case BulletType.Bomb:
                    Instantiate(explosionEffect, transform.position, Quaternion.identity);
                    GameController.DamagePlayer(2);
                    Destroy(gameObject);
                    break;

            }
        }
        else if (col.tag == "Shield" && !isEnemyBullet)
        {
            Destroy(gameObject);
        }
        else if (col.tag == "Wall" && bulletType != BulletType.Projectile)  
        {
            // 地图墙未加tag
            Destroy(gameObject);
        }
        else if (col.tag == "Jar" && !isEnemyBullet)
        {
            col.gameObject.transform.parent.Find("Broken" + col.name).gameObject.SetActive(true);
            col.gameObject.SetActive(false);
        }
    }


    IEnumerator ResetThunder(Enemy enemy)
    {
        yield return new WaitForSeconds(2.0f);
        enemy.RecoverMovingSpeed();
    }

    IEnumerator ResetWater(Enemy enemy, float coolDown)
    {
        yield return new WaitForSeconds(2.0f);
        enemy.RecoverMovingSpeed();
        enemy.SetCoolDownTime(coolDown);
    }

    IEnumerator ContinuousDamage(Enemy enemy)
    {
        int times = Mathf.FloorToInt(damage);
        for(int i = 0; i < times; ++i)
        {
            enemy.getHurt(damage / times);
            yield return new WaitForSeconds(0.7f);
        }
    }
}
