using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnStay : MonoBehaviour
{
    public int damagePerSecond = 1;
    private float elapsed;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player enters fire/magma");
            var roomName = gameObject.transform.parent.parent.GetComponent<Room>().name;
            if (roomName == "Room")
            {
                return;
            }
            elapsed = 0f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            elapsed += Time.deltaTime;
            //Debug.Log("fire " + elapsed);
            var roomName = gameObject.transform.parent.parent.GetComponent<Room>().name;
            if (roomName == "Room")
            {
                return;
            }
            if (elapsed >= 1f)
            {
                GameController.DamagePlayer(damagePerSecond);
                Debug.Log("Fire damage");
                elapsed -= 1f;
            }
        }
    }
}
