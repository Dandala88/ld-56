using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

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

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        gfx = GetComponentInChildren<GFX>();
        audioSource = GetComponent<AudioSource>();
    }

    protected void Update()
    {
        if(ps != null && !ps.IsAlive() && dying)
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
}
