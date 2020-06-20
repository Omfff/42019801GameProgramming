using System;
using UnityEngine;
using System.Collections;
public class BombEnemy:FollowingEnemyAi
{

    public GameObject bulletPrefab;

    protected override void Death()
    {
        base.Death();
    }

    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<BulletController>().GetPlayer(player.transform);
        bullet.GetComponent<Rigidbody2D>().angularVelocity = -100f;
        bullet.GetComponent<BulletController>().isEnemyBullet = true;
        StartCoroutine(CoolDown());   
    }
}
