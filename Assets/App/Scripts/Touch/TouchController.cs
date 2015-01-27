using UnityEngine;
using System.Collections.Generic;

public class TouchController : MonoBehaviour {
    [SerializeField] int rayDistance = 1000;

    private Camera cam;
    private Ray ray;
    private List<RaycastHit> hitList;
    private List<GameObject> selectedList;
    private List<TouchReceiver> touchRecieverList;
    private List<Vector3> worldPositionList;
    private int touchId = -1;
    private Vector3 point;

    void Awake() {
        cam = GetComponent<Camera>();
        Reset();
    }

    void Update () {
        GetTouchState();
    }

    private void GetTouchState() {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
            if(Input.touchCount == 1) {
                touchId = Input.GetTouch(0).fingerId;
                point = new Vector3(Input.GetTouch(0).position.x + Screen.width,Input.GetTouch(0).position.y,0) ;
                Debug.Log("single touch" + point);
            } else if(Input.touchCount > 1) {
                foreach(Touch touch in Input.touches) {
                    if(touch.fingerId == touchId || touchId == -1) {
                        touchId = touch.fingerId;
                        point = touch.position;
                    }
                    Debug.Log("multi touch:" + touch.fingerId);
                }

            } else {
                touchId = -1;
                point = Input.mousePosition;
                Debug.Log("mouse" + point);
            }
            ray = cam.ScreenPointToRay(point);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);
            hitList = new List<RaycastHit>();

            foreach(RaycastHit hit in hits) {
                if(hit.collider != null) {
                    hitList.Add(hit);
                }
            }

            if(hitList.Count > 0) {
                bool isSame = true;
                for(int i = 0, l = selectedList.Count; i < l; i++) {
                    if(hitList.Count < selectedList.Count
                        || (hitList[i].collider != null
                            && selectedList[i] != hitList[i].collider.gameObject)
                    ) {
                        isSame = false;
                        break;
                    }
                }
                if(!isSame && selectedList.Count > 0
                   && touchRecieverList.Count > 0
                ) {
                    for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                        touchRecieverList[i].RecieveLeave(worldPositionList[i], Input.mousePosition, i, l);
                    }
                    Reset();
                } else {
                    Reset();
//                    Debug.Log(hitList.Count);
                    foreach(RaycastHit hit in hitList) {
                        GameObject obj = hit.collider.gameObject;
                        TouchReceiver receiver = obj.GetComponent<TouchReceiver>();

                        if(receiver != null) {
                            selectedList.Add(obj);
                            touchRecieverList.Add(receiver);
                            worldPositionList.Add(hit.point);
                        }
                    }
                }
            } else {
                if(touchRecieverList.Count > 0) {
                    for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                        touchRecieverList[i].RecieveLeave(worldPositionList[i], point, i, l);
                    }
                }
                Reset();
            }
        }
        if(touchRecieverList.Count < 1
            || touchRecieverList.Count != worldPositionList.Count) {return;}

        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("start");
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchRecieverList[i].RecieveStart(worldPositionList[i], point, i, l);
            }
        }
        if(Input.GetMouseButton(0)) {
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchRecieverList[i].RecieveMove(worldPositionList[i], point, i, l);
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchId = -1;
                touchRecieverList[i].RecieveEnd(worldPositionList[i], point, i, l);
            }
            Reset();
        }
    }

    private void Reset() {
        selectedList = new List<GameObject>();
        touchRecieverList = new List<TouchReceiver>();
        worldPositionList = new List<Vector3>();
    }
}
