using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class FollowingEnemyAi : Enemy
{
    const float minSpeed = 0.00001f;

    private float wanderTimer;

    private bool dead = false;

    protected bool coolDownAttack = false;

    protected NavMeshAgent agent;

    protected bool isSlowDown = false;

    //protected float slowDownRate;

    protected Vector3 slowDownVelocity;

    public FollowingEnemyAiData extraAttributes;

    protected float range;

    protected float coolDown;

    protected float wanderRadius;

    protected float wanderChangeInterval;

    protected float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        range = extraAttributes.range;
        coolDown = extraAttributes.coolDown;
        wanderRadius = extraAttributes.wanderRadius;
        wanderChangeInterval = extraAttributes.wanderChangeInterval;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        wanderTimer = wanderChangeInterval;

        stopDistance = attackRange / 3 - 0.5f > 1f ? attackRange / 3 - 0.5f : 1f;

        if (!agent.isOnNavMesh)
        {
            agent.Warp(transform.position);
            Debug.Log("Enemy start: agent is not on navmesh");
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
                if(!CouldAttackPlayer())
                    changeStateToFollow();
            }
            else if (!GameController.instance.isPlayerVisible || !IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }

            if (CouldAttackPlayer())
            {
                currState = EnemyState.Attack;
                if (agent.isOnNavMesh)
                {
                    agent.isStopped = true;
                    setAnimator();
                }
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
        changeDirection();
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
                //Debug.Log("xSpeed:" + agent.velocity.x);
                //Debug.Log("ySpeed:" + agent.velocity.y);
            }
        }
    }

    private void changeDirection()
    {
        if(agent.velocity.x.Equals(0f) && agent.velocity.y.Equals(0f))
        {
            float xDistance = player.transform.position.x - this.transform.position.x;
            float yDistance = player.transform.position.y - this.transform.position.y;
            if(xDistance > 0f)
            {
                if (System.Math.Abs(xDistance) > System.Math.Abs(yDistance))
                {
                    animator.CrossFade("Idle Right", 0f);
                }
                else
                {
                    if (yDistance > 0)
                    {
                        animator.CrossFade("Idle Up", 0f);
                    }
                    else
                    {
                        animator.CrossFade("Idle Down", 0f);
                    }
                }
            }
            else
            {
                if (System.Math.Abs(xDistance) > System.Math.Abs(yDistance))
                {
                    animator.CrossFade("Idle Left", 0f);
                }
                else
                {
                    if (yDistance > 0)
                    {
                        animator.CrossFade("Idle Up", 0f);
                    }
                    else
                    {
                        animator.CrossFade("Idle Down", 0f);
                    }
                }
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
            agent.stoppingDistance = stopDistance;
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
        if (Vector3.Distance(transform.position, player.transform.position) >= stopDistance)
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

    //判断是否能攻击到敌人，有时候在attackRange内但是中间有墙挡着
    protected bool CouldAttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
        {
            return false;
        }
        else if (!GameController.instance.isPlayerVisible)
        {
            return false;
        }
        else
        {
            RaycastHit2D[] hitInfoList = Physics2D.RaycastAll(transform.position,
               (player.transform.position - transform.position).normalized);

            RaycastHit2D hitInfo;

            for (int i = 0; i < hitInfoList.Length; i++)
            {
                hitInfo = hitInfoList[i];
                if (hitInfo.transform.tag == "Enemy" || hitInfo.transform.tag == "Bullet"
                    || hitInfo.transform.tag == "Swamp" || hitInfo.transform.tag == "Spike")
                {
                    continue;
                }

                if (hitInfo.transform.tag == "Player" || hitInfo.transform.tag == "Follower")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }

}
