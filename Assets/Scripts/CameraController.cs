using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5.0f;
    public Vector3 offset;

    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        if (target != null && offset == Vector3.zero)
        {
            offset = transform.position - target.position;
        }
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, -60f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        transform.position = target.position + rotation * offset;

        transform.LookAt(target);
        target.rotation = transform.rotation;
    }
}
