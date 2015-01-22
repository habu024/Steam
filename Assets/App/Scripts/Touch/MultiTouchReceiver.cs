using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class MultiTouchReceiver : MonoBehaviour {
    TouchData data = new TouchData();
    Vector3 startWorldPos;
    float startTime;

    public void RecieveStart(Vector3 worldPos, Vector3 screenPos, int index, int len) {
        startWorldPos = worldPos;
        startTime = Time.time;

        data.startPosition = worldPos;
        data.worldPosition = worldPos;
        data.screenPosition = screenPos;
        data.distance = 0.0f;
        data.distanceX = 0.0f;
        data.distanceY = 0.0f;
        data.angle = 0.0f;
        data.direction = null;
        data.index = index;
        data.length = len;

        SendMessage("Touch", data, SendMessageOptions.DontRequireReceiver);
    }

    public void RecieveEnd(Vector3 worldPos, Vector3 screenPos, int index, int len) {
        data.worldPosition = worldPos;
        data.screenPosition = screenPos;
        data.index = index;
        data.length = len;

        if(Time.time - startTime < 1.0f) {
            if(data.distance < 0.1f) {
                SendMessage("Tap", data, SendMessageOptions.DontRequireReceiver);
            } else {
                SendMessage("Swipe", data, SendMessageOptions.DontRequireReceiver);
            }
        }
        SendMessage("Release", data, SendMessageOptions.DontRequireReceiver);
    }
    
    public void RecieveLeave(Vector3 worldPos, Vector3 screenPos, int index, int len) {
        data.worldPosition = worldPos;
        data.screenPosition = screenPos;
        data.index = index;
        data.length = len;

        SendMessage("Leave", data, SendMessageOptions.DontRequireReceiver);
    }

    public void RecieveMove(Vector3 worldPos, Vector3 screenPos, int index, int len) {
        float disX = worldPos.x - startWorldPos.x;
        float disY = worldPos.y - startWorldPos.y;
        float angle = Mathf.Atan2(disY, disX) * 180 / Mathf.PI;

        data.worldPosition = worldPos;
        data.screenPosition = screenPos;
        data.distance = Mathf.Sqrt(disX * disX + disY * disY);
        data.distanceX = disX;
        data.distanceY = disY;
        data.angle = angle;
        data.index = index;
        data.length = len;
        if(angle >= 45 && angle <= 135) {
            data.direction = "up";
        } else if(angle >= -135 && angle <= -45) {
            data.direction = "down";
        } else if(angle > -45 && angle < 45) {
            data.direction = "right";
        } else {
            data.direction = "left";
        }

        SendMessage("Drag", data, SendMessageOptions.DontRequireReceiver);
    }
}

public class MultiTouchData {
    public Vector3 startPosition;
    public Vector3 worldPosition;
    public Vector3 screenPosition;
    public float distance;
    public float distanceX;
    public float distanceY;
    public float angle;
    public string direction;
    public int index;
    public int length;
}
