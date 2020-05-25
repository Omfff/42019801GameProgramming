using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorCollider;

    private GameObject player;

    private float widthOffset = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            Room currRoom = CameraController.instance.currRoom;
            Vector3 roomCentre = currRoom.GetRoomCentre();
            if (player.transform.position.x + 2f < roomCentre.x)
            {
                Debug.Log("Player enter the left door");
                player.transform.position = new Vector2(player.transform.position.x - widthOffset, player.transform.position.y);
            }
            else if (player.transform.position.x - 2f > roomCentre.x)
            {
                Debug.Log("Player enter the right door");
                player.transform.position = new Vector2(player.transform.position.x + widthOffset, player.transform.position.y);
            }
            else if (player.transform.position.y + 2f < roomCentre.y)
            {
                Debug.Log("Player enter the bottom door");
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y - widthOffset);
            }
            else if (player.transform.position.y - 2f > roomCentre.y)
            {
                Debug.Log("Player enter the top door");
                player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + widthOffset);
            }
        }
    }
}
