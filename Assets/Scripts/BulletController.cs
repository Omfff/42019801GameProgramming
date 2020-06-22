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

    Cruise,

    Sword
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

    public float angle;

    public float radius;

    public int damageTimes = 1;

    private Rigidbody2D rigidbody;
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
            case BulletType.Cruise:
                Player = GameObject.FindGameObjectWithTag("Player");
                bulletSpeed = 400f;
                radius = 1.5f;
                angle = Random.Range(0, 360);
                damageTimes = 4;
                break;
            case BulletType.Sword:
                Player = GameObject.FindGameObjectWithTag("Player");
                bulletSpeed = 430f;
                radius = 1.0f;
                angle = 0;
                break;
        }
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
            acceleration = -1f;
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
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
        else
        {
            switch (bulletType)
            {
                case BulletType.Bomb:
                    StartCoroutine(PlayerBombCountdown());
                    break;
                case BulletType.Cruise:
                    angle += bulletSpeed * Time.deltaTime % 360;
                    radius += Random.Range(-1f, 1f) * Time.deltaTime;
                    float posX = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
                    float posY = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

                    transform.position = new Vector3(posX, posY, 0) + Player.transform.position;
                    break;
                
                 
                case BulletType.Sword:
                    angle += bulletSpeed * Time.deltaTime;
                    //Vector3 pos = RotateRound(transform.position, Player.transform.position, Vector3.forward, angle);
                    //transform.position = pos;
                    transform.Rotate(new Vector3(0, 0, bulletSpeed * Time.deltaTime));
                    //Debug.Log("Rotation:  " + transform.rotation.eulerAngles);
                    //transform.position = new Vector3();
                    radius += Random.Range(-0.1f, 0.1f) * Time.deltaTime;
                    radius = radius < 0.5f ? 0.5f : radius;
                    float x = -radius * Mathf.Cos((transform.rotation.eulerAngles.z - 45f) * Mathf.Deg2Rad);
                    float y = -radius * Mathf.Sin((transform.rotation.eulerAngles.z - 45f) * Mathf.Deg2Rad);
                    transform.position = new Vector3(Player.transform.position.x + x, Player.transform.position.y + y, 0);
                    //Debug.Log("Position:  " + transform.position);
                    //Debug.Log("x, y:      " + x + ", " + y);
                    if (angle > 180f)
                    {
                        Destroy(gameObject);
                    }
                    break;
                default:
                    /*
                    rigidbody.velocity += new Vector2(
                        (rigidbody.velocity.x == 0f ? 1f : 0.1f) * Random.Range(-5f, 5f),
                        (rigidbody.velocity.y == 0f ? 1f : 0.1f) * Random.Range(-5f, 5f));
                    */
                    //rigidbody.velocity += new Vector2(Mathf.Cos(lifeTime*Mathf.Deg2Rad), Mathf.Sin(lifeTime * Mathf.Deg2Rad));
                    //lifeTime -= Time.deltaTime;
                    rigidbody.velocity += rigidbody.velocity * acceleration * Time.deltaTime;

                    break;
            }
        }
    }


    public Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
    {
        Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
        Vector3 resultVec3 = center + point;
        return resultVec3;
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
            else if(collider.transform.tag == "Follower")
            {
                Familiar.instance.Hurt(2);
            }
        }

        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }


    IEnumerator PlayerBombCountdown()
    {
        yield return new WaitForSeconds(lifeTime);

        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, gameObject.GetComponent<CircleCollider2D>().bounds.size.x);
        Collider2D collider;
        for (int i = 0; i < colliders.Length; i++)
        {
            collider = colliders[i];
            if (collider.transform.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Enemy>().getHurt(damage);
                GameController.DamagePlayer(Mathf.FloorToInt(damage));
            }
        }

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
            if (--damageTimes <= 0 && bulletType != BulletType.Sword)
            {
                Destroy(gameObject);
            }
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
        else if (col.tag == "Wall" && bulletType != BulletType.Projectile && bulletType != BulletType.Cruise && bulletType != BulletType.Sword)  
        {
            Destroy(gameObject);
        }
        else if (col.tag == "Jar" && !isEnemyBullet)
        {
            col.gameObject.transform.parent.Find("Broken" + col.name).gameObject.SetActive(true);
            col.gameObject.SetActive(false);
        }else if(col.tag == "Follower" && isEnemyBullet)
        {
            Familiar.instance.Hurt(1);
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
