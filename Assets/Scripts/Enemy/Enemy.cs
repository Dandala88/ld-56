using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health;
    public float experience;
    public int collisionDamage;

    public bool dying;
    protected Rigidbody rb;
    protected ParticleSystem ps;
    protected GFX gfx;
    public Collider hurtBox;
    protected AudioSource audioSource;
    public Image healthbar;
    private float maxHealth;
    protected bool aggro;
    protected Bear bear;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        gfx = GetComponentInChildren<GFX>();
        audioSource = GetComponent<AudioSource>();
        maxHealth = health;
    }

    protected void Update()
    {
        healthbar.transform.position = transform.position + (Vector3.up * 2);
        if(bear != null )
            healthbar.transform.forward = -(bear.transform.position - transform.position);
        healthbar.fillAmount = health / maxHealth;
        if (ps != null && !ps.IsAlive() && dying)
            Destroy(gameObject);
    }

    /// <summary>
    /// returns true if enemy dies on this hit
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool Hurt(float amount)
    {
        if (!dying)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
                return true;
            }
        }
        return false;
    }

    [ContextMenu("KILL")]
    private void Die()
    {
        audioSource.Play();
        gfx.gameObject.SetActive(false);
        hurtBox.enabled = false;
        dying = true;
        ps.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        var bear = other.gameObject.GetComponentInParent<Bear>();
        if (bear != null)
        {
            aggro = true;
            this.bear = bear;
            healthbar.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bear = other.gameObject.GetComponentInParent<Bear>();
        if (bear != null)
        {
            aggro = false;
            this.bear = null;
            healthbar.enabled = false;
        }
    }
}
