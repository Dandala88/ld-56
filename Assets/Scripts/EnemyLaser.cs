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
    private bool alreadyInstantiated;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bear = FindObjectOfType<Bear>();
    }

    private void OnEnable()
    {
        //first script load ignore this code
        if (alreadyInstantiated)
        {
            if (rb != null)
                rb.velocity = transform.forward * speed;
            startPosition = transform.position;
        }
    }

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        startPosition = transform.position;
        alreadyInstantiated = true;
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
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }

        var bear = other.GetComponentInParent<Bear>();
        if (bear != null)
        {
            bear.Hurt(power);
            gameObject.SetActive(false);
        }
    }
}
