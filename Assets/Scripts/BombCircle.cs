 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCircle : MonoBehaviour {

    private GameObject awakeGameOject;

    private void Awake() {
        awakeGameOject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible) {
        awakeGameOject.SetActive(visible);
    }

}
