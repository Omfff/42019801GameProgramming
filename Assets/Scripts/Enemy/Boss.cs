using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : FollowingEnemyAi
{
    protected bool isAttacking;
    public GameObject splitedBossPf;
    public bool isSplittable;
    public float healTime = 3f;
    protected float healInterval = 0.5f;
    protected int direction = 16;

    protected enum BossSkill
    {
        Heal,

        Invincible,

        FullScreenAttack,

        Omnidirection,

        //Unidirection,

        SingleShoot,
    };

    public string BossName;

    public GameObject NormalPrefab;

    public GameObject BombPrefab;

    public GameObject SlalomPrefab;

    public GameObject TrackingPrefab;

    public GameObject ProjectilePrefab;


    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        if (!isAttacking)
        {
            isAttacking = true;
            gameObject.GetComponent<OutlineEffect>().StopOutline();
            BossSkill skill = RandomEnumValue<BossSkill>();
            Debug.Log(skill);
            //skill = BossSkill.Omnidirection;
            switch (skill)
            {
                case BossSkill.Heal:
                    Debug.Log("health" + health);
                    StartCoroutine(SelfHealing());
                    break;
                case BossSkill.Invincible:
                    StartCoroutine(ShieldOpen());
                    break;
                case BossSkill.FullScreenAttack:
                    StartCoroutine(FullScreenAttack());
                    break;
                case BossSkill.Omnidirection:
                    StartCoroutine(OmnidirectionShoot());
                    break;
                    /*
                case BossSkill.Unidirection:
                    StartCoroutine(UnidirectionShoot());
                    break;
                    */
                case BossSkill.SingleShoot:
                    StartCoroutine(SingleShoot());
                    break;
            }
            
        }
    }

    protected IEnumerator ShieldOpen()
    {
        float shieldSize = 6f;
        if(enemyType == EnemyType.SplittedBoss)
        {
            shieldSize = 0.75f;
        }
        gameObject.GetComponent<ShieldController>().Open(shieldSize);
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<ShieldController>().Close();
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    protected IEnumerator SelfHealing()
    {
        float healRemainTime = healTime;
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(new Color(0, 1, 0, 0.7f));
        gameObject.GetComponent<OutlineEffect>().StartOutline();
        while (healRemainTime > 0)
        {
            health += 1;
            Debug.Log("health" + health);
            yield return new WaitForSeconds(healInterval);
            healRemainTime -= healInterval;
        }
        gameObject.GetComponent<OutlineEffect>().StopOutline();
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    protected IEnumerator FullScreenAttack()
    {
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(Color.red);

        gameObject.GetComponent<OutlineEffect>().StartOutline();

        float chargeTime = .5f;
        int i = 0;
        while (chargeTime > 0)
        {
            i++;
            gameObject.GetComponent<OutlineEffect>().StartOutline(1f + i * 2f);
            yield return new WaitForSeconds(1f);
            chargeTime -= .1f;
        }
        //yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<OutlineEffect>().StopOutline();
        ScreenShakeController.instance.StartShake(.4f, .8f);
        GameController.DamagePlayer(2);
        Familiar.instance.Hurt(2);
        yield return new WaitForSeconds(.8f);
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    protected IEnumerator OmnidirectionShoot()
    {
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(Color.blue);

        gameObject.GetComponent<OutlineEffect>().StartOutline();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<OutlineEffect>().StopOutline();

        BulletType bulletType = RandomEnumValue<BulletType>();
        //bulletType = BulletType.Projectile;
        for(int i = 0; i < direction; i++)
        {
            GameObject bullet;
            GameObject _gameObject = new GameObject();
            _gameObject.transform.position = transform.position;
            float dis = (player.transform.position - transform.position).magnitude;
            switch (bulletType)
            {

                case BulletType.Normal:
                    bullet = Instantiate(NormalPrefab, transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Normal;

                    _gameObject.transform.position += new Vector3(Mathf.Cos(i * 22.5f * Mathf.Deg2Rad), Mathf.Sin(i * 22.5f * Mathf.Deg2Rad), 0) * dis;
                    bullet.GetComponent<BulletController>().GetPlayer(_gameObject.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;

                    break;
                case BulletType.Bomb:
                    bullet = Instantiate(BombPrefab, transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Bomb;
                    
                    _gameObject.transform.position += new Vector3(Mathf.Cos(i * 22.5f * Mathf.Deg2Rad), Mathf.Sin(i * 22.5f * Mathf.Deg2Rad), 0) * dis;
                    bullet.GetComponent<BulletController>().GetPlayer(_gameObject.transform);
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;


                    break;
                case BulletType.Slalom:
                    bullet = Instantiate(SlalomPrefab, transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Slalom;
                    
                    _gameObject.transform.position += new Vector3(Mathf.Cos(i * 22.5f * Mathf.Deg2Rad), Mathf.Sin(i * 22.5f * Mathf.Deg2Rad), 0) * dis;
                    bullet.GetComponent<BulletController>().GetPlayer(_gameObject.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;

                    break;
                case BulletType.Tracking:
                    bullet = Instantiate(TrackingPrefab, transform.position + new Vector3(Mathf.Cos(i * 16), Mathf.Sin(i * 16), 0), transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Tracking;
                    bullet.GetComponent<BulletController>().SetPlayer(player);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;

                    yield return new WaitForSeconds(.7f);
                    break;
                case BulletType.Projectile:
                    bullet = Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Projectile;
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0.0f;

                    _gameObject.transform.position += new Vector3(Mathf.Cos(i * 22.5f * Mathf.Deg2Rad), Mathf.Sin(i * 22.5f * Mathf.Deg2Rad), 0) * dis;
                    bullet.GetComponent<BulletController>().GetPlayer(_gameObject.transform);
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;

                    break;
                default:
                    bullet = Instantiate(NormalPrefab, transform.position, transform.rotation) as GameObject;
                    bullet.GetComponent<BulletController>().bulletType = BulletType.Normal;
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;

                    break;
            }
            Destroy(_gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(.8f);
        isAttacking = false;
        StartCoroutine(CoolDown());

    }

    protected IEnumerator UnidirectionShoot()
    {
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(Color.yellow);

        gameObject.GetComponent<OutlineEffect>().StartOutline();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<OutlineEffect>().StopOutline();

    }

    protected IEnumerator SingleShoot()
    {
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(Color.blue);

        gameObject.GetComponent<OutlineEffect>().StartOutline();
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<OutlineEffect>().StopOutline();

        BulletType bulletType = RandomEnumValue<BulletType>();
        GameObject bullet;
        switch (bulletType)
        {

            case BulletType.Normal:
                bullet = Instantiate(NormalPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Normal;

                bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;
            case BulletType.Bomb:
                bullet = Instantiate(BombPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Bomb;

                bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;
            case BulletType.Slalom:
                bullet = Instantiate(SlalomPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Slalom;

                bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;
            case BulletType.Tracking:
                bullet = Instantiate(TrackingPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Tracking;
                bullet.GetComponent<BulletController>().SetPlayer(player);
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;
            case BulletType.Projectile:
                bullet = Instantiate(ProjectilePrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Projectile;

                bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;
            default:
                bullet = Instantiate(NormalPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().bulletType = BulletType.Normal;
                bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                bullet.GetComponent<BulletController>().isEnemyBullet = true;

                break;

        }
        yield return new WaitForSeconds(.8f);
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    protected override void Death()
    {
        if (isSplittable)
        {
            if (currState != EnemyState.Die)
            {
                currState = EnemyState.Die;
            }
            else
            {
                return;
            }
            Debug.Log("boss death");
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            int num = 4;
            Vector3[] generatePos = GenerateSplitedBossPos(transform.position, gameObject.GetComponent<Renderer>().bounds.size.x * 1f, -1, num);
            for (int i = 0; i < num; i++)
            {
                GameObject splitedBoss = Instantiate(splitedBossPf, generatePos[i], Quaternion.identity);
                splitedBoss.transform.parent = RoomController.instance.currRoom.transform;
            }
            StopAllCoroutines();
            Destroy(gameObject);
        }
        else
        {
            base.Death();
        }
       
    }

    protected Vector3[] GenerateSplitedBossPos(Vector3 origin, float dist, int layermask, int num)
    {
        Vector3[] dirList = new Vector3[num];
        Vector3[] generatePos = new Vector3[num];
        Vector3 tempPos;
        for(int i = 0; i < num; i++)
        {
            float ang = 360f / (float)num * i;
            dirList[i] = new Vector3(Mathf.Sin(ang * Mathf.PI / 180) * dist, Mathf.Cos(ang * Mathf.PI / 180) * dist, 0);
            tempPos = dirList[i] + origin;
            NavMeshHit navHit;

            if (NavMesh.SamplePosition(tempPos, out navHit, dist, layermask))
            {
                if (RoomController.instance.isPosInCurrentRoom(tempPos))
                {
                    generatePos[i] = navHit.position;
                }
                else {
                    generatePos[i] = origin;
                }
            }
            else
            {
                generatePos[i] = origin;
            }
            
        }
        return generatePos;

    }

    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(new System.Random().Next(v.Length));
    }

}
