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
    protected Collider collider;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        gfx = GetComponentInChildren<GFX>();
        collider = GetComponentInChildren<Collider>();
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

    private void Die()
    {
        gfx.gameObject.SetActive(false);
        collider.gameObject.SetActive(false);
        dying = true;
        ps.Play();
    }
}
