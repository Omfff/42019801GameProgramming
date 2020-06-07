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
        public SpawnerData spawnerData;
    }

    // public GridController grid;
    public RandomSpawner[] spawnerData;

    void Start()
    {
    }

    public void InitialiseObjectSpawning()
    {
        foreach(RandomSpawner rs in spawnerData)
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {  
        Debug.Log("Spawned Enemy!");
        if (data.spawnerData.name == "BossEnemy")
        {
            GameObject go = Instantiate(data.spawnerData.itemToSpawn, RoomController.instance.getCurrentRoomCenter(), Quaternion.identity, transform) as GameObject;
        }
        else
        {
            int randomNum = Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);
            Vector3[] posList = GenerateRandomPos(RoomController.instance.getCurrentRoomCenter(), RoomController.instance.getCurrentRoomMinRange() / 2 - 1, -1, randomNum);
            for (int i = 0; i < randomNum; i++)
            {
                GameObject go = Instantiate(data.spawnerData.itemToSpawn, posList[i], Quaternion.identity, transform) as GameObject;

            }
        }
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
}
