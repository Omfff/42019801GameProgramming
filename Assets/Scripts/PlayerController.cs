using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerSwapWeapons;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    public float speed;
    Rigidbody2D rigidbody;
    public Text showedText;
    private float lastFire;
    public float fireDelay;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject fireballPrefab;
    public GameObject waterballPrefab;
    public GameObject thunderballPrefab;
    public PlayerSwapWeapons playerSwapWeapons;
    private WeaponType weaponType;
    public bool isHoldingKey { get; set; }

    private float lastUseItem;
    private float lastChangeItem;

    //人物朝向
    private float direction_horizontal;
    private float direction_vertical;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        fireDelay = GameController.FireRate;
        speed = GameController.MoveSpeed;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //记录朝向，否则静止时以上值为0，无法射击
        if(horizontal != 0 || vertical !=0)
        {
            direction_horizontal = horizontal;
            direction_vertical = vertical;
        }
        animator.SetFloat("xSpeed", horizontal);
        animator.SetFloat("ySpeed", vertical);
        //old attack
        //float shootHor = Input.GetAxis("ShootHorizontal");
        //float shootVert = Input.GetAxis("ShootVertical");
        //if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay)
        //{
        //    Shoot(shootHor, shootVert);
        //    lastFire = Time.time;
        //}
        if(Input.GetKey(KeyCode.J) && Time.time > lastFire + fireDelay)
        {
            if (horizontal != 0 || vertical != 0)
            {
                Shoot(horizontal, vertical);
            } else
            {
                Shoot(direction_horizontal, direction_vertical);
            }
            lastFire = Time.time;
        }
        rigidbody.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        //showedText.text = "Nothing to show for now";

        //Skill
        if (Input.GetKey(KeyCode.T))
        {
            GameController.AddShield();
        }
        if (Input.GetKey(KeyCode.Q) && (Time.time > lastChangeItem + GameController.ChangeItemCooldown))
        {
            GameController.ChangeItems();
            lastChangeItem = Time.time;
        }
        if (Input.GetKey(KeyCode.F) && Time.time > lastUseItem + GameController.UseItemCooldown)
        {
            GameController.UseItems();
            lastUseItem = Time.time;
        }
    }

    void Shoot(float x, float y)
    {
        weaponType = playerSwapWeapons.GetWeaponType();
        GameObject bullet;
        for (int i = 0; i < GameController.BulletCount; ++i)
        {
            //Todo: 不能特殊武器时要提示框吗？
            if(GameController.Xp < 1)
            {
                weaponType = WeaponType.Pistol;
            }
            switch (weaponType)
            {

                case WeaponType.Pistol:
                    bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
                    break;
                case WeaponType.Shotgun:
                    bullet = Instantiate(waterballPrefab, transform.position, transform.rotation) as GameObject;
                    GameController.RemoveXp(1);
                    break;
                case WeaponType.Punch:
                    bullet = Instantiate(fireballPrefab, transform.position, transform.rotation) as GameObject;
                    GameController.RemoveXp(1);
                    break;
                case WeaponType.Sword:
                    bullet = Instantiate(thunderballPrefab, transform.position, transform.rotation) as GameObject;
                    GameController.RemoveXp(1);
                    break;
                default:
                    bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
                    break;
            }
            Vector3 v;
            if (x == 0)
            {
                float temp = Mathf.Tan(10 * (i - GameController.BulletCount / 2)* Mathf.Deg2Rad) * y;
                v = new Vector3(temp, y, 0).normalized * bulletSpeed;
            }
            else
            {
                float temp = Mathf.Tan(10 * (i - GameController.BulletCount / 2)) * x;
                v = new Vector3(x, temp, 0).normalized * bulletSpeed;
            }
            bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
            bullet.GetComponent<Rigidbody2D>().velocity = v;
        }


    }
}
