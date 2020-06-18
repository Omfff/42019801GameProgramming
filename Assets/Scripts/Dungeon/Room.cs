using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class Room : MonoBehaviour
{
    public int Width;
    public int Height;
    public int X;
    public int Y;

    private bool updatedDoors = false;
    private NavMeshSurface2d surface;

    public SeperatedDoor leftDoor;
    public SeperatedDoor rightDoor;
    public SeperatedDoor topDoor;
    public SeperatedDoor bottomDoor;
    public List<SeperatedDoor> doors = new List<SeperatedDoor>();

    public GameObject endPoint;

    public Room(int x, int y)
    {
        X = x;
        Y = y;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }
        

        SeperatedDoor[] ds = GetComponentsInChildren<SeperatedDoor>();
        foreach (SeperatedDoor d in ds)
        {
            doors.Add(d);
            switch (d.doorType)
            {
                case SeperatedDoor.DoorType.right:
                    rightDoor = d;
                    break;
                case SeperatedDoor.DoorType.left:
                    leftDoor = d;
                    break;
                case SeperatedDoor.DoorType.top:
                    topDoor = d;
                    break;
                case SeperatedDoor.DoorType.bottom:
                    bottomDoor = d;
                    break;
            }
        }
        if(ds.Length > 0)
        {
            gameObject.GetComponentInChildren<NavMeshSurface2d>().BuildNavMesh();
        }
        RoomController.instance.RegisterRoom(this);
    }

    public void RemoveUnconnectedDoors()
    {
        foreach (SeperatedDoor door in doors)
        {
            switch (door.doorType)
            {
                case SeperatedDoor.DoorType.right:
                    if (GetRoom(1, 0) == null)
                    {
                        //door.gameObject.SetActive(false);
                        Destroy(door.gameObject);
                        Destroy(door.closedDoor.gameObject);
                        door.wall.SetActive(true);
                        door.miniMapWall.SetActive(true);
                    }
                    break;
                case SeperatedDoor.DoorType.left:
                    if (GetRoom(-1, 0) == null)
                    {
                        //door.gameObject.SetActive(false);
                        Destroy(door.gameObject);
                        Destroy(door.closedDoor.gameObject);
                        door.wall.SetActive(true);
                        door.miniMapWall.SetActive(true);
                    }
                    break;
                case SeperatedDoor.DoorType.top:
                    if (GetRoom(0, 1) == null)
                    {
                        //door.gameObject.SetActive(false);
                        Destroy(door.gameObject);
                        Destroy(door.closedDoor.gameObject);
                        door.wall.SetActive(true);
                        door.miniMapWall.SetActive(true);
                    }
                    break;
                case SeperatedDoor.DoorType.bottom:
                    if (GetRoom(0, -1) == null)
                    {
                        //door.gameObject.SetActive(false);
                        Destroy(door.gameObject);
                        Destroy(door.closedDoor.gameObject);
                        door.wall.SetActive(true);
                        door.miniMapWall.SetActive(true);
                    }
                    break;
            }
            // if (door.doorType == SeperatedDoor.DoorType.right && GetRoom(1, 0) == null)
            // {
            //     door.gameObject.SetActive(false);
            //     door.wall.SetActive(true);
            // }
            // else if (door.doorType == SeperatedDoor.DoorType.left && GetRoom(-1, 0) == null)
            // {
            //     door.gameObject.SetActive(false);
            //     door.wall.SetActive(true);
            // }
            // else if (door.doorType == SeperatedDoor.DoorType.top && GetRoom(0, 1) == null)
            // {
            //     door.gameObject.SetActive(false);
            //     door.wall.SetActive(true);
            // }
            // else if (door.doorType == SeperatedDoor.DoorType.bottom && GetRoom(0, -1) == null)
            // {
            //     door.gameObject.SetActive(false);
            //     door.wall.SetActive(true);
            // }
        }
    }

    public Room GetRoom(int xOffset, int yOffset)
    {
        if (RoomController.instance.DoesRoomExist(X + xOffset, Y + yOffset))
        {
            return RoomController.instance.FindRoom(X + xOffset, Y + yOffset);
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (name.Contains("End") && !updatedDoors)
        {
            RemoveUnconnectedDoors();
            updatedDoors = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3( X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RoomController.instance.OnPlayerExitRoom(this);
        }
    }
}
