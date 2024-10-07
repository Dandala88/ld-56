using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Bear : MonoBehaviour
{
    public float maxHealth;
    public float moveSpeed;
    public float maxSpeed;
    public float deceleration;
    public float pitchSpeed;
    public float yawSpeed;
    public float rollSpeed;
    public float hurtKnockback = 500;
    public float hurtITime = 1f;
    public float baseExperience = 10;
    public float experienceExponent = 1.25f;
    public float power;
    public float powerGrowthRate = 0.2f;
    public float rateOfFire;
    public float rateOfFireGrowthRate = 0.2f;
    public float fireDistance;
    public float fireDistanceGrowRate = 2f;
    public Laser laserPrefab;
    public Transform laserOrigin;
    public PauseUI pauseUI;
    public HUD hud;
    public Animator animator;
    public AudioClip laserSound;

    private Vector2 input;
    private Vector2 pitchYaw;
    private float diveSurface;
    private int currentLevel = 1;
    private float currentExperience;
    private float experienceToNextLevel;
    private float currentHealth;
    private bool hurtInvincible;
    private bool inShootCooldown;

    private Rigidbody rb;
    private BearRoot bearRoot;
    private AudioSource audioSource;

    private float startingPower;
    private float startingExperience;
    private float startingRateOfFire;
    private int startingLevel;
    private float startingFireDistance;
    private float startingMaxHealth;
    private bool dead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bearRoot = GetComponentInChildren<BearRoot>();
        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if(GameManager.playerLoaded)
        {
            power = GameManager.playerPower;
            rateOfFire = GameManager.playerRateOfFire;
            currentLevel = GameManager.playerLevel;
            fireDistance = GameManager.playerShootDistance;
            currentExperience = GameManager.playerCurrentExperience;
            maxHealth = GameManager.playerMaxHealth;

            startingPower = GameManager.playerPower;
            startingRateOfFire = GameManager.playerRateOfFire;
            startingLevel = GameManager.playerLevel;
            startingFireDistance = GameManager.playerShootDistance;
            startingExperience = GameManager.playerCurrentExperience;
            startingMaxHealth = GameManager.playerMaxHealth;
        }
        else
        {
            startingPower = power;
            startingRateOfFire = rateOfFire;
            startingLevel = currentLevel;
            startingFireDistance = fireDistance;
            startingExperience = currentExperience;
            startingMaxHealth = maxHealth;
        }

        experienceToNextLevel = CalculateNextLevelExperience(currentLevel, experienceExponent);
        hud.UpdateLevel(currentLevel, rateOfFire, fireDistance, power, maxHealth);
        hud.UpdateHealthBar(currentHealth, maxHealth);
        hud.UpdateExperienceBar(currentExperience, experienceToNextLevel);
        GameManager.playerLoaded = true;
    }

    [ContextMenu("GainLevel")]
    public void GainLevel()
    {
        GainExperience(1000);
    }

    private void Update()
    {
        GameManager.playerLevel = currentLevel;
        GameManager.playerPower = power;
        GameManager.playerShootDistance = fireDistance;
        GameManager.playerCurrentExperience = currentExperience;
        GameManager.playerRateOfFire = rateOfFire;
        GameManager.playerMaxHealth = maxHealth;
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
            rb.velocity += bearRoot.transform.forward * sign * moveSpeed * Time.fixedDeltaTime;
        }

        if (Mathf.Abs(input.x) > 0.1f)
        {
            var sign = Mathf.Sign(input.x);
            rb.velocity += transform.right * sign * moveSpeed * Time.fixedDeltaTime;
        }

        if(diveSurface != 0)
        {
            var sign = Mathf.Sign(diveSurface);
            rb.velocity += transform.up * sign * moveSpeed * Time.fixedDeltaTime;
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
    }

    public void GainExperience(float experience)
    {
        currentExperience += experience;

        if(currentExperience >= experienceToNextLevel)
        {
            currentLevel++;
            maxHealth++;
            currentHealth = maxHealth;
            rateOfFire += rateOfFireGrowthRate;
            power += powerGrowthRate;
            fireDistance += fireDistanceGrowRate;
            var experienceToNextLevelNet = CalculateNextLevelExperience(currentLevel, experienceExponent);
            experienceToNextLevel = experienceToNextLevelNet - currentExperience;
            Debug.Log($"Next Net: {experienceToNextLevelNet} Next adj: { experienceToNextLevel }");
            currentExperience = 0;
            hud.UpdateLevel(currentLevel, rateOfFire, fireDistance, power, maxHealth);
            hud.UpdateHealthBar(currentHealth, maxHealth);
        }

        hud.UpdateExperienceBar(currentExperience, experienceToNextLevel);
    }

    private float CalculateNextLevelExperience(int level, float exponent)
    {
        return baseExperience * Mathf.Pow(level, exponent);
    }

    public void Hurt(float amount)
    {
        if (!hurtInvincible)
        {
            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                StartCoroutine(DeathCoroutine());
            }
            hud.UpdateHealthBar(currentHealth, maxHealth);
            StartCoroutine(HurtInvincibleCoroutine());
        }
    }

    private IEnumerator DeathCoroutine()
    {
        dead = true;
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(3);
        GameManager.playerLevel = startingLevel;
        GameManager.playerPower = startingPower;
        GameManager.playerShootDistance = startingFireDistance;
        GameManager.playerCurrentExperience = startingExperience;
        GameManager.playerRateOfFire = startingRateOfFire;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private bool shooting;
    public void Shoot(InputAction.CallbackContext context)
    {
        if(context.started && !inShootCooldown && !dead)
        {
            shooting = true;
            var clone = Instantiate(laserPrefab);
            clone.transform.position = laserOrigin.position;
            clone.transform.forward = bearRoot.transform.forward;
            clone.power = power;
            clone.distance = fireDistance;
            clone.baseSpeed = rb.velocity.magnitude;
            audioSource.PlayOneShot(laserSound);
            StartCoroutine(ShootCooldownCoroutine());
        }

        if(context.canceled)
        {
            shooting = false;
        }
    }

    private IEnumerator ShootCooldownCoroutine()
    {
        var seconds = 1 / rateOfFire;
        inShootCooldown = true;
        yield return new WaitForSeconds(seconds);
        inShootCooldown = false;
        if (shooting && !dead)
        {
            var clone = Instantiate(laserPrefab);
            clone.transform.position = laserOrigin.position;
            clone.transform.forward = bearRoot.transform.forward;
            clone.power = power;
            clone.distance = fireDistance;
            clone.baseSpeed = rb.velocity.magnitude;
            audioSource.PlayOneShot(laserSound);
            StartCoroutine(ShootCooldownCoroutine());
        }
    }

    public void PitchYaw(InputAction.CallbackContext context)
    {
        pitchYaw = context.ReadValue<Vector2>();
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
