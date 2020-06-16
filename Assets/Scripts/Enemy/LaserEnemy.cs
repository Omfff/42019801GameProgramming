using System;
using System.Collections;
using UnityEngine;
public class LaserEnemy:FollowingEnemyAi
{
    public LineRenderer lineRenderPf;

    private LineRenderer lineRenderer = null;

    public float rayTime = 1.5f;

    private bool isCasting = false;

    private IEnumerator castingCoroutine;

    private void Awake()
    {
        lineRenderer = Instantiate(lineRenderPf, transform.position, Quaternion.identity) as LineRenderer;
        attackRange = 7;
        range = 11;
    }

    protected override void Death()
    {
        if (isCasting)
        {
            StopCoroutine(castingCoroutine);
            lineRenderer.GetComponent<Destroyer>().DestroyMe();
            lineRenderer.enabled = false;

        }
        else
        {
            if (enemyType == EnemyType.Laser)
                lineRenderer.GetComponent<Destroyer>().DestroyMe();
        }
        base.Death();
    }

    protected override void Attack()
    {
        if (coolDownAttack)
            return;
        if (!isCasting)
        {
            castingCoroutine = Laser();
            StartCoroutine(castingCoroutine);
        }
    }

    private IEnumerator Laser()
    {
        float castingTime = 0;
        isCasting = true;
        Vector3 playerPos = player.transform.position;
        //LineRenderer lineRenderer = Instantiate(lineRenderPf, transform.position, Quaternion.identity) as LineRenderer;
        while (castingTime < rayTime)
        {
            if (castingTime == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
            RaycastHit2D[] hitInfoList = Physics2D.RaycastAll(transform.position,
                (playerPos - transform.position).normalized);

            if (hitInfoList != null && hitInfoList.Length > 0)
            {
                RaycastHit2D hitInfo;

                for (int i = 0; i < hitInfoList.Length; i++)
                {

                    hitInfo = hitInfoList[i];
                    if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Bullet" || hitInfo.transform.tag == "Shield")
                    {
                        continue;
                    }
                    if (hitInfo.transform.tag == "Player")
                    {
                        GameController.DamagePlayer(1);
                    }

                    //Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hitInfo.point);
                    break;
                }
            }
            else
            {
                Debug.DrawLine(transform.position, transform.right * 100f, Color.red);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + transform.right * 100);
            }

            lineRenderer.enabled = true;

            yield return new WaitForSeconds(0.1f);

            castingTime += 0.1f;

        }
        lineRenderer.enabled = false;
        isCasting = false;
        StartCoroutine(CoolDown());
    }
}
