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
    public GameObject fogOfWar;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    public List<Room> enteredRooms = new List<Room>();

    bool spawnedBossRoom = false;
    bool updatedRooms = false;
    bool isProceduralGeneration = false;

    bool isLoadingRoom = false;

    private GameObject player;

    private string newWorldName = "";

    public string loadingSceneName = "";

    public static void ReInit()
    {

    }

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
                    //door.gameObject.SetActive(false);
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SwitchWorld(PlayerPrefs.GetString("Level"));
        //SwitchWorld("Hell");
    }

    public void BeginNewWorld() {
        if (loadingSceneName != "")
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(loadingSceneName));
        }

        currentWorldName = newWorldName;
        PlayerPrefs.SetString("Level", currentWorldName);
        if (newWorldName == "Basement")
        {
            GetComponent<DungeonGenerator>().enabled = false;
            LoadRoom("Start", 0, 0);
            LoadRoom("2", 1, 0);
            LoadRoom("3", 2, 0);
            LoadRoom("4", 0, -1);
            LoadRoom("5", 1, -1);
            LoadRoom("6", 0, 1);
            LoadRoom("7", 1, 1);
            LoadRoom("End", 2, 1);
        }
        else
        {
            GetComponent<DungeonGenerator>().enabled = true;
            GetComponent<DungeonGenerator>().StartGeneration();
            spawnedBossRoom = false;
            updatedRooms = false;
            isProceduralGeneration = true;
            player.transform.position = new Vector2(0, 0);
            player.GetComponent<PlayerController>().LetFamiliarFlashToPlayerBeside(0);
        }
    }

    public void SwitchWorld(string worldName)
    {
        DestroyAllRooms();
        if (newWorldName == "")
        {
            newWorldName = worldName;
            BeginNewWorld();
        }
        else
        {
            newWorldName = worldName;
            if (newWorldName.Equals("Forest"))
            {
                loadingSceneName = "AlfheimLoading";
                AsyncOperation loaded = SceneManager.LoadSceneAsync("AlfheimLoading", LoadSceneMode.Additive);
            }
            else if (newWorldName.Equals("Hell"))
            {
                RoomController.instance.fogOfWar.SetActive(true);
                loadingSceneName = "HelheimLoading";
                AsyncOperation loaded = SceneManager.LoadSceneAsync("HelheimLoading", LoadSceneMode.Additive);
            }
            //if (loaded.isDone)
            //{
            //    newWorldName = worldName;
            //    loadingSceneName = "AlfheimLoading";
            //}
        }
    }

    private void DestroyAllRooms()
    {
        foreach (Room room in loadedRooms)
        {
            Destroy(room.gameObject);
        }
        loadedRooms.Clear();
        enteredRooms.Clear();
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
            if (!isProceduralGeneration)
            {
                return;
            }
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRooms)
            {
                foreach (Room room in loadedRooms)
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
        if (loadRoomQueue.Count == 0)
        {
            int maxDistance = 0;
            Room farthestRoom = loadedRooms[0];
            foreach (Room room in loadedRooms)
            {
                int distance = room.X * room.X + room.Y * room.Y;
                if (maxDistance < distance)
                {
                    maxDistance = distance;
                    farthestRoom = room;
                }
            }
            Room tempRoom = new Room(farthestRoom.X, farthestRoom.Y);
            Destroy(farthestRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }

    }

    public void OnPlayerEnterRoom(Room room)
    {
        if (room.name.Contains("Start") || CouldLeaveCurrRoom())
        {
            CameraController.instance.currRoom = room;
            if (!enteredRooms.Contains(room))
            {
                //Debug.Log("Enter room " + room.name);
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
        //CameraController.instance.currRoom = room;
        //currRoom = room;
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
        //CameraController.instance.currRoom = room;
        //currRoom = room;
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
        //StartCoroutine(RoomCoroutine());
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
            currRoom.GetComponent<ObjectRoomSpawner>().InitialiseObjectSpawning();
            if (CouldLeaveCurrRoom())//no enemy (only patrol obstacle)
            {
                OpenDoor();
            }
        }
    }

    private void OpenDoor()
    {
        //open door
        //Debug.Log("Unlock currRoom");
        if (isProceduralGeneration)
        {
            SeperatedDoor[] doors = currRoom.GetComponentsInChildren<SeperatedDoor>();
            foreach (SeperatedDoor door in doors)
            {
                //door.gameObject.SetActive(true);
                door.closedDoor.SetActive(false);
            }
        }
        else
        {
            currRoom.GetComponentInChildren<Door>().doorCollider.SetActive(false);
        }
    }


    private void UpdateCurrentRoom()
    {
        Enemy[] enemies = currRoom.GetComponentsInChildren<Enemy>();
        if (enemies == null || enemies.Length == 0)
        {
            if (!currRoom.GetComponent<ObjectRoomSpawner>().SpawnNextWaveEnemies())
            {
                //open door
                OpenDoor();

                if (currRoom.name.Contains("End"))
                {
                    if (currentWorldName == "Hell")
                    {
                        SceneManager.LoadScene("EndScene");
                    }
                    else
                    {
                        currRoom.endPoint.SetActive(true);
                    }
                }
            }
        }
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
        string[] possibleRooms;
        switch (currentWorldName)
        {
            case "Forest":
                possibleRooms = new string[] { "Empty", "Empty", "Empty", "SpikeAndLaser", "BombAndLaser", "Gears" };
                break;
            case "Hell":
                possibleRooms = new string[] { "Empty", "Empty", "Empty", "LavaSpikeAndGear" };
                break;
            default:
                possibleRooms = new string[] { "Empty" };
                break;
        }

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public bool isPosInCurrentRoom(Vector3 pos)
    {
        Vector3 center = currRoom.GetRoomCentre();
        float xDiff = Mathf.Abs(pos.x - center.x);
        float yDiff = Mathf.Abs(pos.y - center.y);
        // the x distance to the room center is greater than 
        if(xDiff - currRoom.Width/2f < -1.5f && yDiff -currRoom.Height/2f < -1.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}