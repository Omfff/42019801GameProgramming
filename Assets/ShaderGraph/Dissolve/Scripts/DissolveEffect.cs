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

    private void Start() {
        if (material == null) {
            material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        }
    }

    private void Update() {
        if (isDissolving) {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        } else {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }

    public void StartDissolve(float dissolveSpeed, Color dissolveColor) {
        isDissolving = true;
        material.SetColor("_DissolveColor", dissolveColor);
        this.dissolveSpeed = dissolveSpeed;
    }

    public void StopDissolve(float dissolveSpeed, Color dissolveColor) {
        isDissolving = false;
        material.SetColor("_DissolveColor", dissolveColor);
        this.dissolveSpeed = dissolveSpeed;
    }

}
