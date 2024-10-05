using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : MonoBehaviour
{
    public float moveSpeed;
    public float deceleration;
    public float pitchSpeed;
    public float yawSpeed;
    public Camera cam;
    public Laser laserPrefab;
    public Transform laserOrigin;
    private float pitchYawMoveThreshold = 0.25f;

    private Vector2 input;
    private Vector2 pitchYaw;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        var finalRotation = Vector2.zero;
        if (Mathf.Abs(pitchYaw.y) > pitchYawMoveThreshold)
        {
            var sign = Mathf.Sign(pitchYaw.y);
            finalRotation.x = -sign * pitchSpeed * Time.fixedDeltaTime;
        }

        if (Mathf.Abs(pitchYaw.x) > pitchYawMoveThreshold)
        {
            var sign = Mathf.Sign(pitchYaw.x);
            finalRotation.y = sign * yawSpeed * Time.fixedDeltaTime;
        }

        transform.Rotate(finalRotation);

        if (Mathf.Abs(input.y) > 0.1f)
        {
            var sign = Mathf.Sign(input.y);
            rb.AddForce(transform.forward * sign * moveSpeed * Time.fixedDeltaTime);
        }

        if (Mathf.Abs(input.x) > 0.1f)
        {
            var sign = Mathf.Sign(input.x);
            rb.AddForce(transform.right * sign * moveSpeed * Time.fixedDeltaTime);
        }
        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            var clone = Instantiate(laserPrefab);
            clone.transform.position = transform.position;
            clone.transform.forward = transform.forward;
        }
    }

    public void PitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
        pitchYaw.x = Mathf.Clamp(pitchYaw.x, -1f, 1f);
        pitchYaw.y = Mathf.Clamp(pitchYaw.y, -1f, 1f);
    }
}
