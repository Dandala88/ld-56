using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float speed = 30f;
    public float power = 1;
    public float distance = 1f;
    private Vector3 startPosition;
    private float currentDistance;

    private Rigidbody rb;
    private Bear bear;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        bear = FindObjectOfType<Bear>();
        startPosition = transform.position;
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(startPosition, transform.position);
        if (currentDistance > distance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bear = other.GetComponentInParent<Bear>();
        if (bear != null)
        {
            bear.Hurt(power);
            Destroy(gameObject);
        }
    }
}
