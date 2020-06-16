using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class FollowingEnemyAi : Enemy
{
    const float minSpeed = 0.00001f;

    public float range;

    public float coolDown;

    public float wanderRadius;

    public float wanderChangeInterval;

    private float wanderTimer;

    private bool dead = false;

    protected bool coolDownAttack = false;

    protected NavMeshAgent agent;

    protected bool isSlowDown = false;

    //protected float slowDownRate;

    protected Vector3 slowDownVelocity;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        wanderChangeInterval = 3f;
        wanderRadius = range;
        wanderTimer = wanderChangeInterval;

        if (!agent.isOnNavMesh)
        {
            agent.Warp(transform.position);
            Debug.Log("Enemy start: agent is not on navmesh");
        }
        switch (enemyType)
        {
            case (EnemyType.Ranged):
                attackRange = 6;
                range = 10;
                break;
            case (EnemyType.Dash):
                attackRange = 3;
                range = 8;
                coolDown = 2;
                break;
            case EnemyType.Laser:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (EnemyState.Idle):
                break;
            case (EnemyState.Wander):
                Wander();
                if (isSlowDown)
                {
                    agent.velocity = slowDownVelocity;
                }
                break;
            case (EnemyState.Follow):
                Follow();
                if (isSlowDown)
                {
                    agent.velocity = slowDownVelocity;
                }
                break;
            case (EnemyState.Die):
                break;
            case (EnemyState.Attack):
                Attack();
                break;
        }

        if (!notInRoom && !GameController.instance.isTimeStop)
        {

            if (IsPlayerInRange(range) && currState != EnemyState.Die && GameController.instance.isPlayerVisible)
            {
                if(Vector3.Distance(transform.position, player.transform.position) > attackRange)
                    changeStateToFollow();
            }
            else if (!GameController.instance.isPlayerVisible || !IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange && GameController.instance.isPlayerVisible)
            {
                currState = EnemyState.Attack;
                if (agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                }
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }

    private void setAnimator()
    {
        if(System.Math.Abs(agent.velocity.y) > System.Math.Abs(agent.velocity.x))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Left Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Right Walk"))
            {
                if(agent.velocity.y <= 0)
                {
                    animator.CrossFade("Down Walk", 0f);
                }
                else
                {
                    animator.CrossFade("Up Walk", 0f);
                }
            }
            else
            {
                animator.SetFloat("xSpeed", agent.velocity.x);
                animator.SetFloat("ySpeed", agent.velocity.y);
            }
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Down Walk") || animator.GetCurrentAnimatorStateInfo(0).IsName("Up Walk"))
            {
                if (agent.velocity.x <= 0)
                {
                    animator.CrossFade("Left Walk", 0f);
                }
                else
                {
                    animator.CrossFade("Right Walk", 0f);
                }
            }
            else
            {
                animator.SetFloat("xSpeed", agent.velocity.x);
                animator.SetFloat("ySpeed", agent.velocity.y);
            }
        }
    }

    private void Wander()
    {
        agent.stoppingDistance = 0.5f;
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderChangeInterval)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            if (agent.isOnNavMesh)
            {
                agent.SetDestination(newPos);
            }
            else
            {
                Debug.Log("agent not on mesh");
            }
            wanderTimer = 0;
        }

        setAnimator();
        DebugDrawPath(agent.path.corners);

        if (IsPlayerInRange(range))
        {
            changeStateToFollow();
        }
    }

    protected virtual void changeStateToFollow()
    {
        if (agent.isOnNavMesh)
        {
            //agent.ResetPath();
            agent.isStopped = false;
            agent.stoppingDistance = attackRange - 0.5f;
            currState = EnemyState.Follow;
        }
        else
        {
            Debug.Log("changeStateToFollow: agent is not on mesh");
        }
    }

    private void Follow()
    {
        //Debug.Log("Follow");
        if (Vector3.Distance(transform.position, player.transform.position) >= attackRange - 1f)
        {
            agent.SetDestination(player.transform.position);
            setAnimator();
            DebugDrawPath(agent.path.corners);
        }
        else
        {
            agent.isStopped = true;
            animator.CrossFade("Idle Down", 0.1f);
        }
    }

    protected IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public static void DebugDrawPath(Vector3[] corners)
    {
        if (corners.Length < 2) { return; }
        int i = 0;
        for (; i < corners.Length - 1; i++)
        {
            Debug.DrawLine(corners[i], corners[i + 1], Color.blue);
        }
        Debug.DrawLine(corners[0], corners[1], Color.red);
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        while (true)
        {
            Vector3 randDirection = Random.insideUnitSphere * dist;

            randDirection += origin;

            NavMeshHit navHit;

            if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
            {
                return navHit.position;
            }
            else
            {
                Debug.Log("RandomNavSphere: Destination false");
                return navHit.position;  
            }
        }
    }

    public override void SlowDownMovingSpeed(float slowDownRate)
    {
        if (!isSlowDown)
        {
            isSlowDown = true;
            slowDownVelocity = agent.velocity * slowDownRate;
        }
    }

    public override void FrozenMovement()
    {
        SlowDownMovingSpeed(0);
    }

    public override void RecoverMovingSpeed()
    {
        isSlowDown = false;
    }

    public override float GetCoolDownTime()
    {
        return this.coolDown;
    }

    public override void IncreaseAttackCoolTime(float time)
    {
        this.coolDown += time;
    }

    public override void SetCoolDownTime(float time)
    {
        this.coolDown = time;
    }

}
