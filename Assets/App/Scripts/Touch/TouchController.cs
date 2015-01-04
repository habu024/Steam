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

    void Awake() {
        cam = GetComponent<Camera>();
        Reset();
    }

    void Update () {
        GetTouchState();
    }

    private void GetTouchState() {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0)) {
            ray = cam.ScreenPointToRay(Input.mousePosition);
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
                        touchRecieverList[i].RecieveLeave(worldPositionList[i], Input.mousePosition, i, l);
                    }
                }
                Reset();
            }
        }
        if(touchRecieverList.Count < 1
            || touchRecieverList.Count != worldPositionList.Count) {return;}

        if(Input.GetMouseButtonDown(0)) {
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchRecieverList[i].RecieveStart(worldPositionList[i], Input.mousePosition, i, l);
            }
        }
        if(Input.GetMouseButton(0)) {
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchRecieverList[i].RecieveMove(worldPositionList[i], Input.mousePosition, i, l);
            }
        }
        if(Input.GetMouseButtonUp(0)) {
            for(int i = 0, l = touchRecieverList.Count; i < l; i++) {
                touchRecieverList[i].RecieveEnd(worldPositionList[i], Input.mousePosition, i, l);
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
