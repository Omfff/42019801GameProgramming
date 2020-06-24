using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int damagePerTouch = 1;
    private bool spikeUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player enters fire/lava");
            var roomName = gameObject.transform.parent.GetComponent<Room>().name;
            if (roomName == "Room")
            {
                return;
            }
            spikeUp = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var roomName = gameObject.transform.parent.GetComponent<Room>().name;
            if (roomName == "Room")
            {
                return;
            }
            var spikeSprite = GetComponent<SpriteRenderer>().sprite;
            //Debug.Log("Player stays on " + spikeSprite.name);
            if (spikeSprite.name == "spike_2")
            {
                if (spikeUp == false)
                {
                    GameController.DamagePlayer(damagePerTouch);
                    spikeUp = true;
                }
            }
            else
            {
                if (spikeUp == true)
                {
                    spikeUp = false;
                }
            }
        }
    }

}
