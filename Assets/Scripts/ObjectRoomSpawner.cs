using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public Transform[] positionList;
        public SpawnerData spawnerData;
    }

    [System.Serializable]
    public struct EnemySpawner
    {
        public string name;
        public Transform[] positionList;
        public SpawnerData spawnerData;
    }

    [System.Serializable]
    public struct WaveSpanwer
    {
        public EnemySpawner[] enemyBornList;
    }

    public WaveSpanwer[] wavesData; 


    [SerializeField]
    private int currWaveNum;

    void Start()
    {
        currWaveNum = 0;
    }

    public void InitialiseObjectSpawning()
    {
        SpawnWave();
    }

    private Vector3[] GenerateRandomPos(Vector3 origin, float dist, int layermask, int num)
    {
        Vector3[] dirList = new Vector3[num];
        Vector3[] generatePos = new Vector3[num];
        Vector3 tempPos;
        for (int i = 0; i < num; i++)
        {
            float ang = 360f / (float)num * i;
            dirList[i] = new Vector3(Mathf.Sin(ang * Mathf.PI / 180) * dist, Mathf.Cos(ang * Mathf.PI / 180) * dist, 0);
            tempPos = dirList[i] + origin;
            NavMeshHit navHit;

            if (NavMesh.SamplePosition(tempPos, out navHit, dist, layermask))
            {
                generatePos[i] = navHit.position;
            }
            else
            {
                generatePos[i] = origin;
            }
        }
        return generatePos;

    }

    public void SpawnWave()
    {
        foreach (EnemySpawner es in wavesData[currWaveNum].enemyBornList)
        {
            SpawnEnemy(es);
        }
    }

    void SpawnEnemy(EnemySpawner data)
    {
        Debug.Log("Spawned Enemy!");
        if (data.spawnerData.name == "BossEnemy")
        {
            GameObject go = Instantiate(data.spawnerData.itemToSpawn, RoomController.instance.getCurrentRoomCenter(), Quaternion.identity, transform) as GameObject;
        }
        else if (data.spawnerData.name == "PatrolEnemy" && data.positionList.Length > 0)
        {
            GameObject enemy = Instantiate(data.spawnerData.itemToSpawn, data.positionList[0].position, Quaternion.identity, transform) as GameObject;
            enemy.GetComponent<PatrolEnemyAi>().SetPatrolPos(data.positionList);
        }
        else if (data.spawnerData.name == "PatrolObstacle" && data.positionList.Length > 0)
        {
            GameObject obstacle = Instantiate(data.spawnerData.itemToSpawn, data.positionList[0].position, Quaternion.identity, transform) as GameObject;
            obstacle.GetComponent<PatrolObstacle>().SetPatrolPos(data.positionList);
        }
        else
        {
            GameObject enemy = Instantiate(data.spawnerData.itemToSpawn, data.positionList[0].position, Quaternion.identity, transform) as GameObject;
        }
    }

    // if has next wave enemy return true else return false
    public bool SpawnNextWaveEnemies()
    {
        currWaveNum += 1;
        if(currWaveNum < wavesData.Length)
        {
            SpawnWave();
            return true;
        }
        else
        {
            return false;
        }
    }
}
