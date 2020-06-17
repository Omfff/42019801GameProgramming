using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    public Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    public List<Room> enteredRooms = new List<Room>();

    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;
    bool isProceduralGeneration = false; 

    public bool DoesRoomExist(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y) == true)
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom(Room room)
    {
        //Debug.Log("room name:" + room.name);
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform;

            //...
            Transform[] grandFa;
            grandFa = room.GetComponentsInChildren<Transform>();
            foreach (Transform child in grandFa)
            {
                //child.gameObject.layer = LayerMask.NameToLayer("Minimap");
                if (child.gameObject.name == "outline" || child.gameObject.name == "wall2")
                {
                    //child.gameObject.GetComponent<Renderer>().enabled = false;
                }
                if (child.gameObject.name == "outline")
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

                }
            }
            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room;
            }

            // init door closed
            if (isProceduralGeneration) 
            {
                SeperatedDoor[] doors = room.GetComponentsInChildren<SeperatedDoor>();
                foreach (SeperatedDoor door in doors)
                {
                    door.closedDoor.SetActive(true);
                }
            }
            else 
            {
                room.GetComponentInChildren<Door>().doorCollider.SetActive(true);
            }

            loadedRooms.Add(room);
            // room.RemoveUnconnectedDoors();
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // LoadRoom("1", 0, 0);
        // LoadRoom("2", 1, 0);
        // LoadRoom("3", 2, 0);
        // LoadRoom("4", 0, -1);
        // LoadRoom("5", 1, -1);
        // LoadRoom("6", 0, 1);
        // LoadRoom("7", 1, 1);
        // LoadRoom("8", 2, 1);

        currentWorldName = "Forest";
        isProceduralGeneration = true;
        // LoadRoom("Empty", 0, 0);
        // LoadRoom("Empty", 0, -1);
        // LoadRoom("Empty", 0, 1);
        // LoadRoom("Empty", -1, 0);
        // LoadRoom("Empty", 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                //UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void OnPlayerEnterRoom(Room room)
    {
        if (room.name.Contains("Basement-1") || CouldLeaveCurrRoom())
        {
            CameraController.instance.currRoom = room;
            if (!enteredRooms.Contains(room))
            {
                Debug.Log("Enter room " + room.name);
                enteredRooms.Add(room);
                currRoom = room;
                StartCoroutine(RoomCoroutine(1));
            }
            else
            {
                currRoom = room;
            }
        }
        //modify minimap
        // CameraController.instance.currRoom = room;
        // currRoom = room;
        Transform[] grandFa;
        grandFa = room.GetComponentsInChildren<Transform>();
        foreach (Transform child in grandFa)
        {
            //child.gameObject.layer = LayerMask.NameToLayer("Minimap");
            if (child.gameObject.name == "outline" || child.gameObject.name == "wall2")
            {
                child.gameObject.GetComponent<Renderer>().enabled = true;
            }
            if (child.gameObject.name == "outline")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

            }
        }
    }
    public void OnPlayerExitRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
        Transform[] grandFa;
        grandFa = room.GetComponentsInChildren<Transform>();
        foreach (Transform child in grandFa)
        {
            //child.gameObject.layer = LayerMask.NameToLayer("Minimap");
            if (child.gameObject.name == "outline" || child.gameObject.name == "wall2")
            {
                child.gameObject.GetComponent<Renderer>().enabled = true;
            }
            if (child.gameObject.name == "outline")
            {
                child.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;

            }
        }
        // StartCoroutine(RoomCoroutine());
    }
    public bool CouldLeaveCurrRoom()
    {
        Enemy[] enemies = currRoom.GetComponentsInChildren<Enemy>();
        if (enemies.Length == 0)
            return true;
        else
            return false;
    }

    public IEnumerator RoomCoroutine(int type)
    {
        yield return new WaitForSeconds(0.2f);
        if (type == 0) // update current room
        {
            UpdateCurrentRoom();
        }
        else //update entering room
        {
            UpdateEnteringRoom();
        }
    }

    private void UpdateCurrentRoom()
    {
        Enemy[] enemies = currRoom.GetComponentsInChildren<Enemy>();
        if (enemies == null || enemies.Length == 0)
        {
            Debug.Log("Unlock currRoom");
            if (isProceduralGeneration)
            {
                SeperatedDoor[] doors = currRoom.GetComponentsInChildren<SeperatedDoor>();
                foreach (SeperatedDoor door in doors)
                {
                    door.closedDoor.SetActive(false);
                }
            }
            else 
            {
                currRoom.GetComponentInChildren<Door>().doorCollider.SetActive(false);
            }
        }
    }

    private void UpdateEnteringRoom()
    {
        currRoom.GetComponent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }

    public Vector3 getCurrentRoomCenter()
    {
        return currRoom.GetRoomCentre();
    }

    public float getCurrentRoomMinRange()
    {
        return currRoom.Height;
    }

    public string GetRandomRoomName()
    {
        string[] possibleRooms = new string[] {
            "Empty"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

}