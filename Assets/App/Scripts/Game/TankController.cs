using UnityEngine;
using System.Collections.Generic;

public class TankController : MonoBehaviour {
    [SerializeField] GameObject tank;
    [SerializeField] CanonBall canonBall;
    [SerializeField] float speed;
    [SerializeField] GameObject ctrlBase;

    Ray ray;
    float screenRatio;
    Vector3 ctrlPos = new Vector3(0, 0, 0);
    float rad;
    float dis;
    bool isMove = false;
    float shotId = -1;
    float touchId = -1;
    Vector3 shotPosition;
    Vector3 touchPosition;
    Animator animator;

    void Start() {
        screenRatio = Screen.width > Screen.height ? 640f / Screen.height : 640f / Screen.width;
        animator = tank.transform.Find("Sprite").GetComponent<Animator>();
    }

    void Update() {
        if(Input.touchCount > 0) {
            OnTouchPhase();
        } else if(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) {
            OnMousePhase();
        }
    }

    void OnTouchPhase() {
        foreach(Touch touch in Input.touches) {
            if(touch.phase == TouchPhase.Began) {
                if(touchId < 0) {
                    touchId = touch.fingerId;
                    touchPosition = touch.position;
#if UNITY_EDITOR
                    touchPosition.x += Screen.width;
#endif
                    TouchStart();
                } else if(shotId < 0) {
                    shotId = touch.fingerId;
                    shotPosition = touch.position;
#if UNITY_EDITOR
                    shotPosition.x += Screen.width;
#endif
                }
            }
            if(touch.fingerId == touchId) {
                if(touch.fingerId == touchId) {
                    touchPosition = touch.position;
#if UNITY_EDITOR
                    touchPosition.x += Screen.width;
#endif
                    TouchMove();
                }
            }
            if(touch.phase == TouchPhase.Ended) {
                if(touch.fingerId == touchId) {
                    touchPosition = touch.position;
#if UNITY_EDITOR
                    touchPosition.x += Screen.width;
#endif
                    touchId = -1;
                    TouchEnd();
                    if(Vector3.Distance(touchPosition * screenRatio, ctrlPos) < 10f) {
                        shotId = -1;
                        Shot(touchPosition);
                    }
                } else if(touch.fingerId == shotId) {
                    Vector3 curPosition = touch.position;
#if UNITY_EDITOR
                    curPosition.x += Screen.width;
#endif
                    shotId = -1;
                    if(Vector3.Distance(curPosition, shotPosition) < 10f) {
                        Shot(curPosition);
                    }
                }
            }
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
            if(Vector3.Distance(touchPosition * screenRatio, ctrlPos) < 20f) {
                Shot(touchPosition);
            }
        }
    }
    
    void TouchStart() {;
        ctrlPos = touchPosition * screenRatio;
        ctrlBase.transform.localPosition = new Vector3(
            (touchPosition.x - Screen.width  / 2),
            (touchPosition.y - Screen.height / 2),
            0
        );
    }

    void TouchMove() {
        Vector3 pos = touchPosition * screenRatio;
        dis = Vector3.Distance(pos, ctrlPos);
        if(!isMove && dis > 10f) {
            isMove = true;
            ctrlBase.SetActive(true);
        }
        if(isMove) {
            rad = Mathf.Atan2(pos.y - ctrlPos.y, pos.x - ctrlPos.x);
            float angle = rad * Mathf.Rad2Deg;

            if(angle >= -22.5f && angle < 22.5f) {
                animator.SetInteger("moveState", 6);
                rad = 0;
            } else if(angle >= 22.5f && angle < 67.5f) {
                animator.SetInteger("moveState", 5);
                rad = 45 * Mathf.Deg2Rad;
            } else if(angle >= 67.5f && angle < 112.5f) {
                animator.SetInteger("moveState", 4);
                rad = 90 * Mathf.Deg2Rad;
            } else if(angle >= 112.5f && angle < 157.5f) {
                animator.SetInteger("moveState", 3);
                rad = 135 * Mathf.Deg2Rad;
            } else if(angle >= -157.5f && angle < -112.5f) {
                animator.SetInteger("moveState", 1);
                rad = 225 * Mathf.Deg2Rad;
            } else if(angle >= -112.5f && angle < -67.5f) {
                animator.SetInteger("moveState", 0);
                rad = 270 * Mathf.Deg2Rad;
            } else if(angle >= -67.5f && angle < 0) {
                animator.SetInteger("moveState", 7);
                rad = 315 * Mathf.Deg2Rad;
            } else {
                animator.SetInteger("moveState", 2);
                rad = 180 * Mathf.Deg2Rad;
            }
            Vector3 oldPos = tank.transform.position;
            Vector3 newPos = new Vector3(
                oldPos.x + speed * Mathf.Cos(rad) * (dis > 60 ? 60 : dis),
                0,
                oldPos.z + speed * Mathf.Sin(rad) * (dis > 60 ? 60 : dis)
            );
            tank.transform.position = newPos;
//            tank.transform.rotation = Quaternion.AngleAxis(angle, Vector3.down);
        }
        tank.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void TouchEnd() {
        isMove = false;
        ctrlBase.SetActive(false);
    }

    void Shot(Vector3 pos) {
        ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 500);
        foreach(RaycastHit hit in hits) {
            GameObject obj = hit.collider.gameObject;
            TankController receiver = obj.GetComponent<TankController>();
            if(receiver != null) {
                Vector3 tankPos = tank.transform.position;
                float angle = Mathf.Atan2(hit.point.z - tankPos.z, hit.point.x - tankPos.x);
                CanonBall c = Instantiate(
                    canonBall,
                    new Vector3(tankPos.x, tankPos.y + 1.0f, tankPos.z),
                    Quaternion.Euler(0, 0, 0)
                ) as CanonBall;
                c.GetComponent<Rigidbody>().AddForce(
                    new Vector3(30 * Mathf.Cos(angle), 0, 30 * Mathf.Sin(angle)),
                    ForceMode.Impulse
                );
            }
        }
    }
}
