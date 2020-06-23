using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolObstacle : MonoBehaviour
{
    [SerializeField] protected Transform[] destinationList;
    protected int currDestinationIndex;
    protected int destinationNum;
    protected PatrolDir patrolDir = PatrolDir.Go;
    protected bool isHurtPlayerInPatrol;
    protected float patrolHurtingCoolTime;
    public float speed;


    // Use this for initialization
    void Start()
    {
        currDestinationIndex = 1;
        patrolHurtingCoolTime = 1f;
    }


    private void Update()
    {
        if (destinationList != null && destinationList.Length > 0)
        {
            Move();
            Rotate();
        }
    }

    public void SetPatrolPos(Transform[] desList)
    {
        destinationList = desList;
        destinationNum = desList.Length;
    }

    protected void Move()
    {
        if (Vector2.Distance(transform.position, destinationList[currDestinationIndex].position) <= 0.1f)
        {
            if (patrolDir == PatrolDir.Go)
            {
                currDestinationIndex += 1;
            }
            else
            {
                currDestinationIndex -= 1;
            }

        }
        if (currDestinationIndex >= destinationNum)
        {
            if (destinationList[0].position == destinationList[destinationNum - 1].position)
            {
                currDestinationIndex = 1;
                patrolDir = PatrolDir.Go;
            }
            else
            {
                currDestinationIndex -= 2;
                patrolDir = PatrolDir.Return;
            }
        }
        else if (currDestinationIndex < 0)
        {
            currDestinationIndex += 2;
            patrolDir = PatrolDir.Go;
        }
        transform.position = Vector2.MoveTowards(transform.position, destinationList[currDestinationIndex].position, speed * Time.deltaTime);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.forward, 180 * Time.deltaTime);
    }

    protected IEnumerator CooldownHurtingPlayer()
    {
        isHurtPlayerInPatrol = true;
        yield return new WaitForSeconds(patrolHurtingCoolTime);
        isHurtPlayerInPatrol = false;
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHurtPlayerInPatrol && collision.gameObject.tag == "Player")
        {
            GameController.DamagePlayer(1);
            StartCoroutine(CooldownHurtingPlayer());
        }
    }
}
