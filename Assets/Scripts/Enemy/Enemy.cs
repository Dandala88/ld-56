using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float experience;
    public int collisionDamage;

    protected bool dying;
    protected Rigidbody rb;
    protected ParticleSystem ps;
    protected GFX gfx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<ParticleSystem>();
        gfx = GetComponentInChildren<GFX>();
    }

    protected void Update()
    {
        if(!ps.IsAlive() && dying)
            Destroy(gameObject);
    }

    public void Hurt(int amount)
    {
        if (!dying)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        gfx.gameObject.SetActive(false);
        dying = true;
        ps.Play();
    }
}
