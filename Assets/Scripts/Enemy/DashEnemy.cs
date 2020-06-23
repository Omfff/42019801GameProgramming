using System;
using UnityEngine;
using System.Collections;
public class DashEnemy:FollowingEnemyAi
{
    private bool isDashing = false;

    private bool isHurtPlayerInDashing = false;

    private IEnumerator dashCoroutine;

    protected override void Death()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
        base.Death();
    }

    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        if (!isDashing)
        {
            dashCoroutine = Dash();
            StartCoroutine(dashCoroutine);
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        Vector3 targetPos = player.transform.position;
        Vector3 curPos = transform.position;
        while (Vector3.Distance(curPos, targetPos) > 0.1f)
        {
            RaycastHit2D[] hitInfoList = Physics2D.RaycastAll(transform.position,
                (targetPos - transform.position).normalized);

            if (hitInfoList != null && hitInfoList.Length > 0)
            {
                RaycastHit2D hitInfo;

                for (int i = 0; i < hitInfoList.Length; i++)
                {

                    hitInfo = hitInfoList[i];
                    if (hitInfo.transform.tag == "Bullet")
                    {
                        continue;
                    }

                    //Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
                    targetPos = hitInfo.point;
                    //Debug.Log("dashing type "+ hitInfo.transform.tag);
                    break;
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 5 * speed * Time.deltaTime);
            //Debug.Log("dash " + targetPos);
            //Debug.Log("dash" + Vector3.Distance(curPos, targetPos));
            yield return null;
            curPos = transform.position;
        }
        ////Debug.Log("dash finish");
        endDash();
    }

    private void endDash()
    {
        //Debug.Log("end dash");
        isDashing = false;
        isHurtPlayerInDashing = false;
        StartCoroutine(CoolDown());
    }

    protected override void changeStateToFollow()
    {
        if (dashCoroutine != null && isDashing)
        {
            StopCoroutine(dashCoroutine);
            endDash(); 
        }
        //gameObject.GetComponent<ShieldController>().Open(1.5f);
        base.changeStateToFollow();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && !isHurtPlayerInDashing && collision.gameObject.tag == "Player")
        {
            //Debug.Log("dash collision");
            GameController.DamagePlayer(1);
            isHurtPlayerInDashing = true;
            StopCoroutine(dashCoroutine);
            endDash();
            //gameObject.GetComponent<ShieldController>().Close();
        }
        base.OnCollisionEnter2D(collision);
    }

}
