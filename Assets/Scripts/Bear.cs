using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private Vector2 input;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 targetMovement = new Vector3(input.x, 0f, input.y);
        rb.velocity = targetMovement * moveSpeed * Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}
