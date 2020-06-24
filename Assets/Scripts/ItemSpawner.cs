using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight;
    }

    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;

    void Awake()
    {
        totalWeight = 0;
        foreach(var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;

        while(pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }

        GameObject i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity) as GameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
<<<<<<< HEAD
=======

    public void dropItemAftherEnemyDeath(Vector3 position)
    {
        if(Random.Range(0.0f, 1.0f) > 0.5f)
        {
            float weight = Random.Range(0.0f, 1.0f);
            float temp = 0;
            int itemIndex = 0;
            for (int i = 0; i < items.Count; ++i)
            {
                temp += items[i].weight;
                if (temp < weight)
                {
                    if (temp + items[i + 1].weight > weight)
                    {
                        itemIndex = i + 1;
                        break;
                    }
                }
                else
                {
                    itemIndex = i;
                    break;
                }
            }

            if (itemIndex < items.Count)
            {
                GameObject i = Instantiate(items[itemIndex].gameObject, position, Quaternion.identity) as GameObject;
            }
        }
    }

    public void dropTreasure(Vector3 position)
    {
        int itemIndex = items.Count - 1;
        GameObject i = Instantiate(items[itemIndex].gameObject, position, Quaternion.identity) as GameObject;
    }
>>>>>>> omf
}
