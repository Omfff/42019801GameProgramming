using UnityEngine;

[CreateAssetMenu(fileName = "FollowingEnemyAiData.asset", menuName = "Enemies/FollowingEnemyAiAttributes")]
public class FollowingEnemyAiData : ScriptableObject
{
    public float range;

    public float coolDown;

    public float wanderRadius;

    public float wanderChangeInterval;
}
