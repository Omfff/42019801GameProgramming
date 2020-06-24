using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    private GameObject player;

    public GameObject openedDoor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && player.GetComponent<PlayerController>().isHoldingKey == true)
        {
            this.gameObject.SetActive(false);
            openedDoor.SetActive(true);
            player.GetComponent<PlayerController>().KeyStateChange(false);
        }
    }
}
