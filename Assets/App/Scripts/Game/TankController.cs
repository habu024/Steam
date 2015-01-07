﻿using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
    [SerializeField] GameObject tank;
    [SerializeField] float speed;
    float screenRatio;
    Vector3 ctrlPos = new Vector3(120f, 120f, 0);
    float rad;
    float dis;
    bool isTouch = false;
    bool isTouchCtrl = false;

    void Start() {
        screenRatio = Screen.width > Screen.height ? 640f / Screen.height : 640f / Screen.width;
    }

    void Update() {
        if(isTouchCtrl) {
            Vector3 oldPos = tank.transform.position;
            Vector3 newPos = new Vector3(
                oldPos.x + speed * Mathf.Cos(rad) * dis,
                0,
                oldPos.z + speed * Mathf.Sin(rad) * dis
            );
            float angle = rad * Mathf.Rad2Deg - 90;
            tank.rigidbody.velocity = Vector3.zero;
            tank.transform.position = newPos;
            tank.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }
    }

    void Touch(TouchData data) {
        Debug.Log(data.screenPosition * screenRatio);
        isTouch = true;
    }
    
    void Drag(TouchData data) {
        Vector3 pos = data.screenPosition * screenRatio;
        dis = Vector3.Distance(pos, ctrlPos);
        if(dis < 110) {
            isTouchCtrl = true;
            rad = Mathf.Atan2(pos.y - ctrlPos.y, pos.x - ctrlPos.x);
        }  else {
            isTouchCtrl = false;
        }
    }

    void Leave(TouchData data) {
        Debug.Log("Leave");
        isTouch = false;
        isTouchCtrl = false;
    }

    void Release(TouchData data) {
        Debug.Log("Release");
        isTouch = false;
        isTouchCtrl = false;
    }
}
