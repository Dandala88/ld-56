using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 30f;
    public float power = 1;
    public float distance = 1f;
    private Vector3 startPosition;
    private float currentDistance;
    public float baseSpeed;

    private Rigidbody rb;
    private Bear bear;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * (speed + baseSpeed);
        bear = FindObjectOfType<Bear>();
        startPosition = transform.position;
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(startPosition, transform.position);
        if(currentDistance > distance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }

        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null && !other.isTrigger)
        {
            var enemyDead = enemy.Hurt(power);
            if(enemyDead)
                bear.GainExperience(enemy.experience);
        }
    }  
}
