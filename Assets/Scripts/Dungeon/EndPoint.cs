using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public string nextWorldName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Move to next world!");
            RoomController.instance.SwitchWorld(nextWorldName);
            //RoomController.instance.NextWorld();
        }
    }
}
