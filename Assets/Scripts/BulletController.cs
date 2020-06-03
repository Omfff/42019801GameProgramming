using System.Collections;
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

    public bool projectileStart;
    // Start is called before the first frame update
    void Start() 
    {
        //StartCoroutine(DeathDelay());
        switch (bulletType)
        {
            case BulletType.Normal:
                bulletSpeed = 8f;
                StartCoroutine(DeathDelay());
                break;
            case BulletType.Bomb:
                bulletSpeed = 2f;
                break;
            case BulletType.Slalom:
                bulletSpeed = 7f;
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
                    if (slalomFirst)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, destPos, bulletSpeed * Time.deltaTime);
                    }
                    else
                    {
                        transform.position = Vector2.MoveTowards(transform.position, startPos, bulletSpeed * Time.deltaTime);

                    }
                    if (curPos == destPos)
                    {
                        StartCoroutine(SlalomDelay());
                    }
                    if (curPos == startPos && !slalomFirst)
                    {
                        Destroy(gameObject);
                    }
                    lastPos = curPos;
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
                        gameObject.AddComponent<Rigidbody2D>().gravityScale = 0.0f;
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
        yield return new WaitForSeconds(.2f);
        slalomFirst = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<Enemy>().getHurt(damage);
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
        else if (col.tag == "Untagged" && bulletType == BulletType.Projectile) 
        {
            Debug.Log("untagged is "+isEnemyBullet);
        }
    }

}
