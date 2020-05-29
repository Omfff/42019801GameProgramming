using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextPopUp : MonoBehaviour
{
    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private static int sortingOrder;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    public void Setup(int damageAmount)
    { 
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;

        disappearTimer = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(.7f,1) * 5f;

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        
    }

    private void Update()
    { 
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime; 

        if(disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;

            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if(textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
