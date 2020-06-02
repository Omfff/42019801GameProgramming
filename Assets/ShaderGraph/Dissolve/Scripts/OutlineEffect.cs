using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineEffect : MonoBehaviour
{
    [SerializeField] private Material material;

    private float thinkness = 0;
    private Color outlineColor = new Color(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_OutlineThickness", thinkness);
        material.SetColor("_OutlineColor", outlineColor);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (thinkness > 0)
        {
            material.SetFloat("_OutlineThickness", thinkness);
            material.SetColor("_OutlineColor", outlineColor);
        }
        
    }

    public void StartOutline()
    {
        thinkness = 1.0f;
        material.SetFloat("_OutlineThickness", thinkness);
        material.SetColor("_OutlineColor", Color.red);
    }

    public void StopOutline()
    {
        thinkness = 0.0f;
        material.SetFloat("OutlineThickness", thinkness);
        material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
    }

    public void SetOutlineColor(Color color)
    {
        outlineColor = color;
    }
}
