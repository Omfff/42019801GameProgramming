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

    Attack,

    Patrol
};

public enum EnemyType
{
    Dash,

    Ranged,

    Shooting,

    Laser,

    Boss,

    SplittedBoss,

    Patrol
};

public class Enemy : MonoBehaviour
{
    const float minSpeed = 0.00001f;

    protected Animator animator;

    protected GameObject player;

    public EnemyState currState = EnemyState.Idle;

    public EnemyType enemyType;

    public float health;

    public float speed;

    public bool notInRoom = false;

    public GameObject deathEffect;

    public GameObject damageTextPf;


    // Start is called before the first frame update
    protected void Init()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.queriesStartInColliders = false;

        Material material = new Material(Shader.Find("Shader Graphs/Outline"));
        gameObject.GetComponent<MaterialTintColor>().SetTintMaterial(material);
        gameObject.GetComponent<SpriteRenderer>().material = material;

        switch (enemyType)
        {
            case (EnemyType.Ranged):
                break;
            case (EnemyType.Dash):
                break;
            case EnemyType.Laser:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
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

    protected bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
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
        }
        else
        {
            Death();
        }
    }

    public virtual void FrozenMovement(){}

    public virtual void SlowDownMovingSpeed(float slowDownRate){}

    public virtual void RecoverMovingSpeed(){}

    public virtual float GetCoolDownTime()
    {
        return 0;
    }

    public virtual void SetCoolDownTime(float time){}

    public virtual void IncreaseAttackCoolTime(float time){}

    protected virtual void OnCollisionEnter2D(Collision2D collision){}
}
