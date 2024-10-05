using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float speed = 75f;
    public int power = 1;
    public float lifetime = 1f;

    private float lifeElapsed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        lifeElapsed += Time.deltaTime;
        if (lifeElapsed > lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.Hurt(power);
            Destroy(gameObject);
        }
    }  
}
