/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Teleporter : MonoBehaviour {

    [SerializeField] private Transform teleportToTransform;

    [ColorUsageAttribute(true, true)]
    [SerializeField] private Color startDissolveColor;
    [ColorUsageAttribute(true, true)]
    [SerializeField] private Color stopDissolveColor;

    private void OnTriggerEnter2D(Collider2D collider) {
        Player player = collider.GetComponent<Player>();
        if (player != null) {
            float dissolveSpeed = .5f;
            float teleportTime = 1f / dissolveSpeed;

            DissolveEffect dissolveEffect = collider.GetComponent<DissolveEffect>();
            dissolveEffect.StartDissolve(dissolveSpeed, startDissolveColor);

            FunctionTimer.Create(() => {
                player.TeleportTo(teleportToTransform.position + new Vector3(0, 2));
                FunctionTimer.Create(() => dissolveEffect.StopDissolve(dissolveSpeed, stopDissolveColor), .2f);
            }, teleportTime);
        }
    }
}*/
