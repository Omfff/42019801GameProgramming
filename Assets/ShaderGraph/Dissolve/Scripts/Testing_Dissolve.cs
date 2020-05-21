/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Dissolve : MonoBehaviour {

    [SerializeField] private DissolveEffect dissolveEffect;
    
    [ColorUsageAttribute(true, true)]
    [SerializeField] private Color startDissolveColor;
    [ColorUsageAttribute(true, true)]
    [SerializeField] private Color stopDissolveColor;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            dissolveEffect.StartDissolve(.7f, startDissolveColor);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            dissolveEffect.StopDissolve(.7f, stopDissolveColor);
        }
    }

}
