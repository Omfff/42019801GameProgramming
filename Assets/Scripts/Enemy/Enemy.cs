using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,

    Wander,

    Follow,

    Die,

    Attack
};

public enum EnemyType
{
    Dash,

    Ranged,

    Shooting,

    Laser,

    Boss
};

public class Enemy : MonoBehaviour
{
    const float minSpeed = 0.00001f;

    Animator animator;

    protected GameObject player;

    public EnemyState currState = EnemyState.Idle;

    public EnemyType enemyType;

    public float health;

    public float range;

    public float speed;

    public float attackRange;

    public float coolDown;

    public float wanderRadius;

    public float wanderChangeInterval;

    private float wanderTimer;

    private bool dead = false;

    protected bool coolDownAttack = false;

    private Material matDefault;

    private Material matWhite;

    SpriteRenderer spriteRender;

    public bool notInRoom = false;

    public GameObject deathEffect;

    public GameObject damageTextPf;

    protected NavMeshAgent agent;

    //private int[] dirX = { 0, 0, -1, 1 };
    //private int[] dirY = { 1, -1, 0, 0 };

    //public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.queriesStartInColliders = false;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        wanderChangeInterval = 3f;
        wanderRadius = range;
        wanderTimer = wanderChangeInterval;

        Material material = new Material(Shader.Find("Shader Graphs/Outline"));
        gameObject.GetComponent<MaterialTintColor>().SetTintMaterial(material);
        gameObject.GetComponent<SpriteRenderer>().material = material;

        if (!agent.isOnNavMesh)
        {
            agent.Warp(transform.position);
            Debug.Log("Enemy start: agent is not on navmesh");
        }
        //switch (enemyType)
        //{
        //    case (EnemyType.Melee):
        //        attackRange = 1;
        //        break;
        //    case (EnemyType.Ranged):
        //        attackRange = 6;
        //        range = 10;
        //        break;
        //    case (EnemyType.Dash):
        //        attackRange = 4;
        //        range = 8;
        //        coolDown = 2;
        //        break;
        //    case EnemyType.Laser:

        //        break;
        //}
        //spriteRender = GetComponent<SpriteRenderer>();
        //matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
        //matDefault = spriteRender.material;
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
                break;
            case (EnemyState.Follow):
                Follow();
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

    protected virtual void Death()
    {
        if (enemyType == EnemyType.Boss)
        {
            Debug.Log("boss enemy death");
        }

        if (currState != EnemyState.Die)
        {
            currState = EnemyState.Die;
        }
        else
        {
            return;
        }

        Instantiate(deathEffect, transform.position, Quaternion.identity);

        GameObject itemSpawner = GameObject.FindGameObjectWithTag("ItemSpawner");
        itemSpawner.GetComponent<ItemSpawner>().dropItemAftherEnemyDeath(transform.position);

        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine(0));

        Destroy(gameObject);

    }

    protected virtual void Attack()
    {
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    protected IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void getHurt(float damage)
    {
        health -= damage;

        GameObject textPopUp = Instantiate(damageTextPf, transform.position, Quaternion.identity);
        DamageTextPopUp damageText = textPopUp.GetComponent<DamageTextPopUp>();
        damageText.Setup((int)damage);

        //spriteRender.material = matWhite;
        if (health > 0)
        {
            // hurt effect:splash
            gameObject.GetComponent<MaterialTintColor>().SetTintFadeSpeed(6f);
            gameObject.GetComponent<MaterialTintColor>().SetTintColor(Color.red);
            //Invoke("ResetMaterial", 0.1f);
        }
        else
        {
            Death();
        }
    }

    private void ResetMaterial()
    {
        spriteRender.material = matDefault;
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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
  
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (enemyType == EnemyType.Dash && isDashing && !isHurtPlayerInDashing)
    //    {
    //        GameController.DamagePlayer(1);
    //        isHurtPlayerInDashing = true;
    //    }
    //}

}
