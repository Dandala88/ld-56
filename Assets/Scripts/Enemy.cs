using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

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
