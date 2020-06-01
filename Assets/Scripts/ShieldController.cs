using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldPf;
    private GameObject shield;
    public float defaultSize = 1;
    // Start is called before the first frame update
    void Start()
    {
        shield = Instantiate(shieldPf, transform.position, Quaternion.identity);
        shield.transform.SetParent(transform,true);
        //shield = transform.Find("ShieldParticle").gameObject;
        shield.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        shield.GetComponent<CircleCollider2D>().radius = defaultSize;
        //Open(defaultSize);
    }

    //gameObject.GetComponent<ShieldController>().Open(1.5f);
    public void Open(float startSize)
    {
        var shieldMain = shield.GetComponent<ParticleSystem>().main;
        shieldMain.startSize = startSize;
        shield.GetComponent<ParticleSystem>().Play();
        shield.GetComponent<CircleCollider2D>().radius = startSize/2;
    }

    // Usage :  gameObject.GetComponent<ShieldController>().Close();
    public void Close()
    {
        shield.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        shield.GetComponent<CircleCollider2D>().radius = 0;
    }
}
