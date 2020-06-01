using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum BossSkill
{
    Heal,

    Invincible,

    FullScreenAttack
};
public class Boss : Enemy
{
    private bool isAttacking;
    public GameObject splitedBossPf;
    public float healTime = 3f;
    private float healInterval = 0.5f;
    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        if (!isAttacking)
        {
            isAttacking = true;
            //gameObject.GetComponent<OutlineEffect>().StopOutline();
            BossSkill skill = RandomEnumValue<BossSkill>();
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
            }
            
        }
    }

    private IEnumerator ShieldOpen()
    {
        gameObject.GetComponent<ShieldController>().Open(3f);
        yield return new WaitForSeconds(3f);
        gameObject.GetComponent<ShieldController>().Close();
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    private IEnumerator SelfHealing()
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

    private IEnumerator FullScreenAttack()
    {
        gameObject.GetComponent<OutlineEffect>().SetOutlineColor(Color.red);

        gameObject.GetComponent<OutlineEffect>().StartOutline();
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<OutlineEffect>().StopOutline();

        ScreenShakeController.instance.StartShake(.4f, .8f);
        GameController.DamagePlayer(2);
        yield return new WaitForSeconds(.8f);
        isAttacking = false;
        StartCoroutine(CoolDown());
    }

    protected override void Death()
    {
        int num = 4;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Vector3[] generatePos = GenerateSplitedBossPos(transform.position, gameObject.GetComponent<Renderer>().bounds.size.x * 1.5f, -1, num);
        for (int i = 0; i < num; i++)
        {
            Instantiate(splitedBossPf, generatePos[i], Quaternion.identity);
        }
       
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private Vector3[] GenerateSplitedBossPos(Vector3 origin, float dist, int layermask, int num)
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
                generatePos[i] = navHit.position;
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
