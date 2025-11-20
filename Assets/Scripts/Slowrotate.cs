using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    // Very slow speed (try 2.0f or 5.0f)
    public float rotationSpeed = 5.0f;

    void Update()
    {
        // Rotate around the Z axis (forward/backward axis)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}