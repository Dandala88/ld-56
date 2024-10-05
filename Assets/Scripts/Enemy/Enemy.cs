using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    protected Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Hurt(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }    
    }

    private void Die()
    {
        Destroy(gameObject);

    }
}
