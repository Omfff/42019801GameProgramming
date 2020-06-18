using UnityEngine;

[CreateAssetMenu(fileName = "BaseEnemyAttributes.asset", menuName = "Enemies/BaseEnemyAttributes")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;

    public float health;

    public float speed;

    public float attackRange;

    public GameObject deathEffect;

    public GameObject damageTextPf;
}
