using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 input;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 inputVector = new Vector3(input.x, 0f, input.y);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, transform.forward);

        // Apply the rotation to the vector
        Vector3 rotatedVector = rotation * inputVector;
        rb.velocity = rotatedVector * moveSpeed * Time.fixedDeltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
