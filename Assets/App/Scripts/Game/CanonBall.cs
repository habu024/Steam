using UnityEngine;
using System.Collections;

public class CanonBall : MonoBehaviour {
    float timer = 0;

    // Use this for initialization
    void Start() {
    
    }
    
    // Update is called once per frame
    void FixedUpdate() {
        timer += Time.deltaTime;
        float v = Mathf.Sqrt(
            (gameObject.GetComponent<Rigidbody>().velocity.x * gameObject.GetComponent<Rigidbody>().velocity.x) +
            (gameObject.GetComponent<Rigidbody>().velocity.z * gameObject.GetComponent<Rigidbody>().velocity.z)
        );
        if(timer > 0.1f && v < 20) {
            Destroy(gameObject);
        }
    }
}
