using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
    [SerializeField] GameObject tank;
    [SerializeField] float speed;
    [SerializeField] UISprite ctrlBase;

    float screenRatio;
    Vector3 ctrlPos = new Vector3(0, 0, 0);
    float rad;
    float dis;
    bool isTouch = false;
    bool isMove = false;
    float touchId = -1;
    Vector3 touchPosition;

    void Start() {
        screenRatio = Screen.width > Screen.height ? 640f / Screen.height : 640f / Screen.width;
    }

    void Update() {
        if(Input.touchCount > 0) {
            OnTouchPhase();
        } else if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            OnMousePhase();
        }

//        if(Input.touchCount == 1) {
//            touchId = Input.GetTouch(0).fingerId;
//            touchPosition = new Vector3(Input.GetTouch(0).position.x + Screen.width,Input.GetTouch(0).position.y,0) ;
//            Debug.Log("single touch" + touchPosition);
//            if(Input.touches[0].phase == TouchPhase.Began) {
//                TouchStart();
//            }
//        } else if(Input.touchCount > 1) {
//            foreach(Touch touch in Input.touches) {
//                if(touch.fingerId == touchId || touchId == -1) {
//                    touchId = touch.fingerId;
//                    touchPosition = touch.position;
//                }
//                Debug.Log("multi touch:" + touch.fingerId);
//            }
//        } else {
//            touchId = -1;
//            touchPosition = Input.mousePosition;
//            Debug.Log("mouse" + touchPosition);
//            if(Input.GetMouseButtonDown(0)) {
//                TouchStart();
//            } else if(Input.GetMouseButton(0)) {
//                TouchMove();
//            }
//
//        }

//        if(isMove) {
//            Vector3 oldPos = tank.transform.position;
//            Vector3 newPos = new Vector3(
//                oldPos.x + speed * Mathf.Cos(rad) * (dis > 110 ? 110 : dis),
//                0,
//                oldPos.z + speed * Mathf.Sin(rad) * (dis > 110 ? 110 : dis)
//            );
//            float angle = rad * Mathf.Rad2Deg - 90;
//            tank.transform.position = newPos;
//            tank.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
//        }
//        tank.rigidbody.velocity = Vector3.zero;
    }

    void OnTouchPhase() {
        if(Input.touchCount == 1) {
            touchPosition = Input.touches[0].position;
            touchPosition.x += Screen.width;
            if(Input.touches[0].phase == TouchPhase.Began) {
                touchId = Input.GetTouch(0).fingerId;
                TouchStart();
//            } else if(Input.touches[0].phase == TouchPhase.Moved) {
//                TouchMove();
            } else if(Input.touches[0].phase == TouchPhase.Ended) {
                TouchEnd();
            }
            TouchMove();
        } else {

        }
    }

    void OnMousePhase() {
        touchId = -1;
        touchPosition = Input.mousePosition;
        if(Input.GetMouseButtonDown(0)) {
            TouchStart();
        } else if(Input.GetMouseButton(0)) {
            TouchMove();
        } else if(Input.GetMouseButtonUp(0)) {
            TouchEnd();
        }
    }
    

    
    void TouchStart() {
        isTouch = true;
        ctrlPos = touchPosition * screenRatio;
        ctrlBase.transform.localPosition = new Vector3(
            (touchPosition.x - Screen.width  / 2) * screenRatio,
            (touchPosition.y - Screen.height / 2) * screenRatio,
            0
        );
    }

    void TouchMove() {
        Vector3 pos = touchPosition * screenRatio;
        dis = Vector3.Distance(pos, ctrlPos);

        if(!isMove && dis > 0.05f) {
            isMove = true;
            ctrlBase.gameObject.SetActive(true);
        }
        if(isMove) {
            rad = Mathf.Atan2(pos.y - ctrlPos.y, pos.x - ctrlPos.x);
            Vector3 oldPos = tank.transform.position;
            Vector3 newPos = new Vector3(
                oldPos.x + speed * Mathf.Cos(rad) * (dis > 110 ? 110 : dis),
                0,
                oldPos.z + speed * Mathf.Sin(rad) * (dis > 110 ? 110 : dis)
            );
            float angle = rad * Mathf.Rad2Deg - 90;
            tank.transform.position = newPos;
            tank.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }
        tank.rigidbody.velocity = Vector3.zero;
    }
    
    
    
    void TouchEnd() {
        Debug.Log("Release");
        isTouch = false;
        isMove = false;
        ctrlBase.gameObject.SetActive(false);
    }
}
