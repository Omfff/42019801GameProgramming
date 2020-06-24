using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AddNavmeshOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    public NavMeshData navMeshData;

    private NavMeshDataInstance mInstance;

    private void Start()
    {
        if (navMeshData != null)
        {
            mInstance = NavMesh.AddNavMeshData(navMeshData, transform.position, transform.rotation);
            //Debug.Log("Add nav mesh data");
        }
    }

    private void OnDestroy()
    {
        mInstance.Remove();
    }
}
