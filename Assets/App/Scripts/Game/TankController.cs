using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
    float screenRatio;

    void Start() {
        screenRatio = Screen.width > Screen.height ? 640f / Screen.height : 640f / Screen.width;
    }
    
    // Update is called once per frame
    void Update() {
    
    }
    
    void Touch(TouchData data) {
        Debug.Log(data.screenPosition * screenRatio);
    }
}
