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
    public float rollSpeed;
    public Laser laserPrefab;
    public Transform laserOrigin;
    public PauseUI pauseUI;

    private Vector2 input;
    private Vector2 pitchYaw;
    private float diveSurface;

    private Rigidbody rb;
    private BearRoot bearRoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bearRoot = GetComponentInChildren<BearRoot>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        var finalRotation = Vector3.zero;

        finalRotation.x = -pitchYaw.y * pitchSpeed * Time.fixedDeltaTime;
        finalRotation.y = pitchYaw.x * yawSpeed * Time.fixedDeltaTime;
        //Pitch on root so that it does not cause gimbal crazies
        bearRoot.transform.Rotate(Vector3.right * finalRotation.x);
        transform.Rotate(Vector3.up * finalRotation.y);

        if (Mathf.Abs(input.y) > 0.1f)
        {
            var sign = Mathf.Sign(input.y);
            rb.AddForce(bearRoot.transform.forward * sign * moveSpeed * Time.fixedDeltaTime);
        }

        if (Mathf.Abs(input.x) > 0.1f)
        {
            var sign = Mathf.Sign(input.x);
            rb.AddForce(transform.right * sign * moveSpeed * Time.fixedDeltaTime);
        }

        if(diveSurface != 0)
        {
            var sign = Mathf.Sign(diveSurface);
            rb.AddForce(transform.up * sign * moveSpeed * Time.fixedDeltaTime);
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
            clone.transform.position = laserOrigin.position;
            clone.transform.forward = bearRoot.transform.forward;
        }
    }

    public void PitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
        pitchYaw.x = Mathf.Clamp(pitchYaw.x, -1f, 1f);
        pitchYaw.y = Mathf.Clamp(pitchYaw.y, -1f, 1f);
    }

    public void DiveSurface(InputAction.CallbackContext context)
    {
        diveSurface = context.ReadValue<float>();
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (pauseUI.paused)
                pauseUI.ContinueGame();
            else
                pauseUI.gameObject.SetActive(true);
        }
    }
}
