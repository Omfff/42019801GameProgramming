using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FamiliarState
{
    Active,

    Notactive,

    Idel
}
public class Familiar : MonoBehaviour
{
    public static Familiar instance;
    private float lastFire;
    private GameObject player;
    public FamiliarData familiar;
    private float lastOffsetX;
    private float lastOffsetY;
    private Rigidbody2D rigidbody;
    [SerializeField]
    private float currHealth;
    [SerializeField]
    private FamiliarState currState;
    private void Awake()
    {
        instance = this;
        currState = FamiliarState.Active;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currHealth = familiar.maxHealth;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(Healing());
        StartCoroutine(Shoot());
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if(horizontal != 0 || vertical != 0)
        {
            float offsetX = (horizontal < 0) ? Mathf.Floor(horizontal) : Mathf.Ceil(horizontal);
            float offsetY = (vertical < 0) ? Mathf.Floor(vertical) : Mathf.Ceil(vertical);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, familiar.speed * Time.deltaTime);
            lastOffsetX = offsetX;
            lastOffsetY = offsetY;
        }
        else
        {
            if(!(transform.position.x < lastOffsetX + 0.5f) || !(transform.position.y < lastOffsetY + 0.5f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - lastOffsetX,
                    player.transform.position.y - lastOffsetY ), familiar.speed * Time.deltaTime);
            }
        }
    }
    public void Hurt(float damage)
    {
        currHealth = currHealth - damage;
        if (currHealth < 0)
        {
            currHealth = 0;
            StartCoroutine(Idel());
        }else if (currHealth <= familiar.maxHealth * familiar.healthPercentageToBeActive)
        {
            currState = FamiliarState.Notactive;
        }
    }

    public void FlashToPlayerBeside(Vector3 pos)
    {
        transform.position = pos;
    }

    Vector3 GetNearestEnemy()
    {
        Vector3 nearestPos = transform.position;
        if (RoomController.instance.currRoom != null)
        {
            Enemy[] enemies = RoomController.instance.currRoom.GetComponentsInChildren<Enemy>();

            float minDis = 1000f;
            //没有敌人则返回自身坐标

            foreach (Enemy e in enemies)
            {
                if (Vector3.Distance(e.transform.position, transform.position) < minDis)
                {
                    minDis = Vector3.Distance(e.transform.position, transform.position);
                    nearestPos = e.transform.position;
                }
            }
        }
        return nearestPos;
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            while (currState == FamiliarState.Active)
            {
                Vector3 pos = GetNearestEnemy();
                if (pos != transform.position)
                {
                    GameObject bullet = Instantiate(familiar.bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    float posX = pos.x - transform.position.x;
                    float posY = pos.y - transform.position.y;
                    bullet.GetComponent<BulletController>().isEnemyBullet = false;
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<Rigidbody2D>().velocity = (new Vector2(posX, posY)).normalized * 10.0f +
                        new Vector2(rigidbody.velocity.x, rigidbody.velocity.y);

                }
                yield return new WaitForSeconds(familiar.fireDelay);
            }
            yield return null;
        }
    }

    private IEnumerator Healing()
    {
        // while player is alive
        while (true)
        {
            if (currState != FamiliarState.Idel) 
            {
                if (currHealth < familiar.maxHealth)
                {
                    currHealth += 1;
                }
                
                if(currHealth > familiar.maxHealth * familiar.healthPercentageToBeActive)
                {
                    currState = FamiliarState.Active;
                }
            }
            yield return new WaitForSeconds(familiar.healInterval);
        }
    }

    private IEnumerator Idel()
    {
        currState = FamiliarState.Idel;
        yield return new WaitForSeconds(familiar.idelTime);
        currState = FamiliarState.Notactive;
    }

    public float GetCurrHealth()
    {
        return currHealth;
    }
}
