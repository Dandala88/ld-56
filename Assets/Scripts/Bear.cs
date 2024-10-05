using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : MonoBehaviour
{
    public float moveSpeed;
    public Camera cam;

    private Vector2 input;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(input.x, 0f, input.y);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.forward);

        // Apply the rotation to the vector
        Vector3 rotatedVector = rotation * inputVector;
        rb.velocity = rotatedVector * moveSpeed * Time.fixedDeltaTime;
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, cam.transform.rotation, 6f * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
