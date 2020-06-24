using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPatrolEnemy : PatrolEnemyAi
{
    private enum LaserDir
    {
        Left,

        Right
    }
    public LineRenderer lineRenderPf;

    private LineRenderer lineRenderer = null;

    public float rayTime = 1.5f;

    private bool isCasting = false;

    private IEnumerator castingCoroutine;

    private void Awake()
    {
        lineRenderer = Instantiate(lineRenderPf, transform.position, Quaternion.identity) as LineRenderer;
    }

    protected override void Attack()
    {
        if (!isCasting)
        {
            castingCoroutine = Laser();
            StartCoroutine(castingCoroutine);
        }
    }
    private Vector3 GetLaserDir(Vector3 targetPos)
    {
        Vector3 p1 = GetPreviousDestination();
        Vector3 p2 = destinationList[currDestinationIndex].position;
        Vector3 a = p2 - p1;
        Vector3 b = targetPos - p1;
        float temp = a.x * b.y - a.y * b.x;
        /// >0，说明点在线的右边，小于在左边，等于则在线上
        if (temp > 0)
        {
            return Quaternion.AngleAxis(90, Vector3.forward) * a;
        }
        else
        {
            return Quaternion.AngleAxis(-90, Vector3.forward) * a;
        }
    }

    private IEnumerator Laser()
    {
        float castingTime = 0;
        isCasting = true;
        Vector3 lDir;
        while (castingTime < rayTime)
        {
            if (castingTime == 0)
            {
                //outline变化啥的
            }
            lDir = GetLaserDir(player.transform.position);
            RaycastHit2D[] hitInfoList = Physics2D.RaycastAll(transform.position,
                lDir.normalized);

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
                    if (hitInfo.transform.tag == "Follower")
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
                Debug.DrawLine(transform.position, lDir.normalized * 100f, Color.red);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + lDir.normalized * 100);
            }

            lineRenderer.enabled = true;

            yield return new WaitForSeconds(0.1f);

            castingTime += 0.1f;

        }
        lineRenderer.enabled = false;
        isCasting = false;
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
            {
                lineRenderer.GetComponent<Destroyer>().DestroyMe();
            }
        }
        base.Death();
    }
}
