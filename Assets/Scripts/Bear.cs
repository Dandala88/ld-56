using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bear : MonoBehaviour
{
    public int maxHealth;
    public float moveSpeed;
    public float deceleration;
    public float pitchSpeed;
    public float yawSpeed;
    public float rollSpeed;
    public float hurtKnockback = 500;
    public float hurtITime = 1f;
    public float baseExperience = 10;
    public float experienceExponent = 1.25f;
    public Laser laserPrefab;
    public Transform laserOrigin;
    public PauseUI pauseUI;
    public HUD hud;

    private Vector2 input;
    private Vector2 pitchYaw;
    private float diveSurface;
    private int currentLevel = 1;
    private float currentExperience;
    private float experienceToNextLevel;
    private int currentHealth;
    private bool hurtInvincible;

    private Rigidbody rb;
    private BearRoot bearRoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bearRoot = GetComponentInChildren<BearRoot>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        experienceToNextLevel = CalculateNextLevelExperience(currentLevel, experienceExponent);
        hud.UpdateLevel(currentLevel);
        hud.UpdateHealthBar(currentHealth, maxHealth);
        hud.UpdateExperienceBar(currentExperience, experienceToNextLevel);
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

    public void GainExperience(float experience)
    {
        currentExperience += experience;

        if(currentExperience >= experienceToNextLevel)
        {
            currentLevel++;
            var experienceToNextLevelNet = CalculateNextLevelExperience(currentLevel, experienceExponent);
            experienceToNextLevel = experienceToNextLevelNet - currentExperience;
            Debug.Log($"Next Net: {experienceToNextLevelNet} Next adj: { experienceToNextLevel }");
            currentExperience = 0;
            hud.UpdateLevel(currentLevel);
        }

        hud.UpdateExperienceBar(currentExperience, experienceToNextLevel);
    }

    private float CalculateNextLevelExperience(int level, float exponent)
    {
        return baseExperience * Mathf.Pow(level, exponent);
    }

    public void Hurt(int amount)
    {
        if (!hurtInvincible)
        {
            currentHealth -= amount;
            hud.UpdateHealthBar(currentHealth, maxHealth);
            StartCoroutine(HurtInvincibleCoroutine());
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponentInParent<Enemy>();
        if(enemy != null && !hurtInvincible)
        {
            rb.AddForce(collision.contacts[0].normal * hurtKnockback);
            Hurt(enemy.collisionDamage);
        }
    }

    private IEnumerator HurtInvincibleCoroutine()
    {
        hurtInvincible = true;
        yield return new WaitForSeconds(hurtITime);
        hurtInvincible = false;
    }
}
