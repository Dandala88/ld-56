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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bear = FindObjectOfType<Bear>();
    }

    private void OnEnable()
    {
        if(rb != null)
            rb.velocity = transform.forward * speed;
            startPosition = transform.position;
    }

    private void Update()
    {
        currentDistance = Vector3.Distance(startPosition, transform.position);
        if (currentDistance > distance)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bear = other.GetComponentInParent<Bear>();
        if (bear != null)
        {
            bear.Hurt(power);
            gameObject.SetActive(false);
        }
    }
}
