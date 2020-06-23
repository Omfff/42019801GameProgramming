using System;
using System.Collections;
using UnityEngine;
public class LaserEnemy:FollowingEnemyAi
{
    public LineRenderer lineRenderPf;

    private LineRenderer lineRenderer = null;

    public LineRenderer aimingRayPf;

    private LineRenderer aimingRayLineRender = null;

    public float rayTime = 1.5f;

    private bool isCasting = false;

    private IEnumerator castingCoroutine;

    private void Awake()
    {
        lineRenderer = Instantiate(lineRenderPf, transform.position, Quaternion.identity) as LineRenderer;
        aimingRayLineRender = Instantiate(aimingRayPf, transform.position, Quaternion.identity) as LineRenderer;
    }

    protected override void Death()
    {
        if (isCasting)
        {
            StopCoroutine(castingCoroutine);
            lineRenderer.GetComponent<Destroyer>().DestroyMe();
            lineRenderer.enabled = false;
            aimingRayLineRender.GetComponent<Destroyer>().DestroyMe();
            aimingRayLineRender.enabled = false;

        }
        else
        {
            if (enemyType == EnemyType.Laser)
            {
                lineRenderer.GetComponent<Destroyer>().DestroyMe();
                aimingRayLineRender.GetComponent<Destroyer>().DestroyMe();
            }
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
                float aimingTime = 0.8f;
                float defaultWidth = aimingRayLineRender.startWidth;
                float currWidth;
                float flashingInterval = 0.2f;
                // line flashing
                while (aimingTime > 0) {
                    aimingRayLineRender.SetPosition(0, transform.position);
                    aimingRayLineRender.SetPosition(1, playerPos);
                    currWidth = aimingRayLineRender.startWidth == 0 ? defaultWidth : 0;
                    aimingRayLineRender.startWidth = currWidth;
                    aimingRayLineRender.endWidth = currWidth;
                    aimingRayLineRender.enabled = true;
                    yield return new WaitForSeconds(flashingInterval);
                    aimingTime -= flashingInterval;
                }
                aimingRayLineRender.startWidth = defaultWidth;
                aimingRayLineRender.endWidth = defaultWidth;
                aimingRayLineRender.enabled = false;
            }
            RaycastHit2D[] hitInfoList = Physics2D.RaycastAll(transform.position,
                (playerPos - transform.position).normalized);

            if (hitInfoList != null && hitInfoList.Length > 0)
            {
                RaycastHit2D hitInfo;

                for (int i = 0; i < hitInfoList.Length; i++)
                {

                    hitInfo = hitInfoList[i];
                    if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Bullet" || hitInfo.transform.tag == "Shield"
                        || hitInfo.transform.tag == "Swamp" || hitInfo.transform.tag == "Item" || hitInfo.transform.tag == "Spike"
                        || hitInfo.transform.tag == "Gear")
                    {
                        continue;
                    }
                    if (hitInfo.transform.tag == "Player")
                    {
                        GameController.DamagePlayer(1);
                    }
                    if(hitInfo.transform.tag == "Follower")
                    {
                        Familiar.instance.Hurt(1f);
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
