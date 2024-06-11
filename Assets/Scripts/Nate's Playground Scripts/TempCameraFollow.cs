using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera should follow
    public Vector3 offset;   // Offset from the target position

    void Update()
    {
        if (target != null)
        {
            // Set the camera's position to the target's position plus the offset
            transform.position = target.position + offset;
        }
    }
}