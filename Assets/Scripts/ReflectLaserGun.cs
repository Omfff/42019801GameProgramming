using System.Collections.Generic;
using UnityEngine;

public class ReflectLaserGun : MonoBehaviour
{
    public float maxDistance = 50;

    public int maxReflectTimes = 10;

    public LineRenderer lineRenderPf;

    private LineRenderer laserLineRender;

    private List<Vector3> renderPoint;

    private void Awake()
    {
        laserLineRender = Instantiate(lineRenderPf, transform.position, Quaternion.identity) as LineRenderer;
        //laserLineRender.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Press O");
            StartCoroutine(ShootLaser());
        }
    }

    private System.Collections.IEnumerator ShootLaser()
    {
        Debug.Log("Laser gun shooting");
        renderPoint = new List<Vector3>();
        renderPoint.Add(transform.position); //LineRenderer以自己为起点

        renderPoint.AddRange(GetRenderPoints(transform.position, transform.right,
            maxDistance, maxReflectTimes));//获取反射点

        Debug.Log("");
        laserLineRender.positionCount = renderPoint.Count;
        laserLineRender.SetPositions(renderPoint.ToArray());
        laserLineRender.enabled = true;
        yield return new WaitForSeconds(0.5f);
        laserLineRender.enabled = false;
    }
    // 开始位置 方向 距离 最大次数
    private List<Vector3> GetRenderPoints(Vector3 start, Vector3 dir, float dis, int times)
    {
        var hitPosList = new List<Vector3>();
        
        while (dis > 0 && times > 0)
        {
            RaycastHit2D hit;
            hit = Physics2D.Raycast(start, dir, dis);
            if(!hit)
                break;
            if(hit.transform.tag != "Player" && hit.transform.tag != "Bullet" && hit.transform.tag != "Follower")
            { 
                hitPosList.Add(hit.point);
                var reflectDir = Vector3.Reflect(dir, hit.normal);
                dir = reflectDir;
            }
           
            dis -= (hit.point - new Vector2(transform.position.x, transform.position.y)).magnitude;
            times--;
            start = hit.point;
            
        }
        return hitPosList;
    }
}