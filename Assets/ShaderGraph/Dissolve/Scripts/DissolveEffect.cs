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

public class DissolveEffect : MonoBehaviour {

    [SerializeField] private Material material;

    private float dissolveAmount;
    private float dissolveSpeed;
    private bool isDissolving;


    private void Update() {
        
        if (isDissolving) {
            Debug.Log("Dissolving");
            dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount > 0.7f ? 0.7f : dissolveAmount);
        } else {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        
    }

    public void StartDissolve(float dissolveSpeed) {
        isDissolving = true;
        this.dissolveSpeed = dissolveSpeed;
        //material.SetFloat("_DissolveAmount", 0.7f);
    }

    public void StopDissolve(float dissolveSpeed) {
        isDissolving = false;
        this.dissolveSpeed = dissolveSpeed;
        //material.SetFloat("_DissolveAmount", 0.0f);

    }

}
