using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Swamp : MonoBehaviour
{
    private bool buff = false;
    public float decelerationRatio = 0.5f;
    public float minSpeed = 2f;
    public float normalSpeed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var speed = GameController.MoveSpeed;
            Debug.Log("Player enters swamp with speed " + speed);
            var roomName = gameObject.transform.parent.parent.GetComponent<Room>().name;
            Debug.Log("swamp in " + roomName);
            if (roomName == "Room")
            {
                return;
            }
            if (!buff)
            {
                var deceleratedSpeed = Math.Max(minSpeed, speed * decelerationRatio);
                //GameController.MoveSpeedChange(deceleratedSpeed);
                GameController.MoveSpeed = deceleratedSpeed;
                buff = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var speed = GameController.MoveSpeed;
            Debug.Log("Player exits swamp with speed" + speed);
            if (buff)
            {
                //GameController.MoveSpeedChange(speed / decelerationRatio);
                //var acceleratedSpeed = Math.Max(normalSpeed, speed / decelerationRatio);
                GameController.MoveSpeed = speed / decelerationRatio;
                buff = false;
            }
        }
    }
}
