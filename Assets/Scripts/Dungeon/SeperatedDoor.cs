using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperatedDoor : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    public GameObject closedDoor;
    public GameObject wall;
    public GameObject miniMapWall;

    private GameObject player;

    private float widthOffset = 9f;
    private float heightOffset = 6f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && RoomController.instance.CouldLeaveCurrRoom())
        {
            Room currRoom = CameraController.instance.currRoom;
            Vector3 roomCentre = currRoom.GetRoomCentre();
            switch(doorType)
            {
                case DoorType.bottom:
                    Debug.Log("Player enter the bottom door");
                    player.transform.position = new Vector2(roomCentre.x, roomCentre.y - heightOffset);
                    break;
                case DoorType.left:
                    Debug.Log("Player enter the left door");
                    player.transform.position = new Vector2(roomCentre.x - widthOffset, roomCentre.y);
                    break;
                case DoorType.right:
                    Debug.Log("Player enter the right door");
                    player.transform.position = new Vector2(roomCentre.x + widthOffset, roomCentre.y);
                    break;
                case DoorType.top:
                    Debug.Log("Player enter the top door");
                    player.transform.position = new Vector2(roomCentre.x, roomCentre.y + heightOffset);
                    break;
            }
        }
    }
}
