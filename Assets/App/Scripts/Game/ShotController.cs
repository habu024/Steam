using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {
    [SerializeField] GameObject tank;
    [SerializeField] CanonBall canonBall;

    Vector3 startPos;

    // Use this for initialization
    void Start() {
    
    }

    // Update is called once per frame
    void Update() {
    
    }

    void Touch(TouchData data) {
        startPos = data.worldPosition;
    }

    void Release(TouchData data) {
        float diffX = startPos.x - data.worldPosition.x;
        float diffZ = startPos.z - data.worldPosition.z;
        if(Mathf.Abs(diffX) < 0.1f && Mathf.Abs(diffZ) < 0.1f) {
            Shot(data.worldPosition);
        }
    }

    void Shot(Vector3 pos) {
        Vector3 tankPos = tank.transform.position;
        float angle = Mathf.Atan2(pos.z - tankPos.z, pos.x - tankPos.x);
        Debug.Log("angle: " + Mathf.Rad2Deg * angle);
        CanonBall c = Instantiate(
            canonBall,
            new Vector3(tankPos.x, tankPos.y + 1.0f, tankPos.z),
            Quaternion.Euler(0, 0, 0)
        ) as CanonBall;
        c.rigidbody.AddForce(new Vector3(10 * Mathf.Cos(angle), 0, 10 * Mathf.Sin(angle)), ForceMode.Impulse);
    }
}
