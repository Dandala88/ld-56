using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVirus : Enemy
{
    public float shootInterval = 1f;
    public float radius = 30f;
    public float deceleration;
    public float rotationSpeed;
    public EnemyLaser laserPrefab;
    public Turret turretPrefab;

    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();
    private bool aggro;
    private Bear bear;

    protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GenerateTurrets();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach (var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    private void Update()
    {
        base.Update();

        if (aggro)
        {
            shootElapsed += Time.deltaTime;

            if (shootElapsed > shootInterval)
            {
                shootElapsed = 0;
                Shoot();
            }
        }

        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        transform.Rotate(rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime);
    }

    private void Shoot()
    {
        foreach (var turret in turrets)
        {
            var clone = Instantiate(laserPrefab);
            clone.transform.position = turret.transform.position;
            if (bear != null)
                clone.transform.forward = (bear.transform.position - transform.position).normalized;
            else
                clone.transform.forward = turret.transform.forward;
        }
    }

    private void GenerateTurrets()
    {
        var segments = 11;
        var horizontalSegments = 14;
        for (int i = 0; i <= segments; i++)
        {
            float phi = Mathf.PI * (i + 0.5f) / segments;

            for (int j = 0; j < horizontalSegments; j++)
            {
                float theta = 2 * Mathf.PI * j / horizontalSegments;

                float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = radius * Mathf.Cos(phi);
                float z = radius * Mathf.Sin(phi) * Mathf.Sin(theta);

                var turret = Instantiate(turretPrefab);
                turret.transform.parent = transform;
                turret.transform.localPosition = new Vector3(x, y, z);

                Vector3 directionFromCenter = turret.transform.localPosition.normalized;
                turret.transform.forward = directionFromCenter;

                Debug.DrawRay(turret.transform.position, transform.forward, Color.yellow, 60);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bear = other.gameObject.GetComponentInParent<Bear>();
        if(bear != null)
        {
            aggro = true;
            this.bear = bear;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var bear = other.gameObject.GetComponentInParent<Bear>();
        if (bear != null)
        {
            aggro = false;
            this.bear = null;
        }
    }
}
