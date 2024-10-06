using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private List<EnemyLaser> laserPool = new List<EnemyLaser>();
    private int laserPoolIterator;

    protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        healthbar.enabled = false;
        GenerateTurrets();
        SetupLaserPool();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach (var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    protected void Update()
    {
        base.Update();
        healthbar.transform.position = transform.position + (Vector3.up * radius * transform.localScale.y * 2);

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
            var clone = laserPool[laserPoolIterator];
            laserPoolIterator++;
            if (laserPoolIterator >= laserPool.Count)
                laserPoolIterator = 0;
            clone.transform.position = turret.transform.position;
            if (bear != null)
                clone.transform.forward = (bear.transform.position - transform.position).normalized;
            else
                clone.transform.forward = turret.transform.forward;
            clone.gameObject.SetActive(true);
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

    private void SetupLaserPool()
    {
        for(int i= 0; i < 1000; i++)
        {
            var clone = Instantiate(laserPrefab);
            clone.gameObject.SetActive(false);
            laserPool.Add(clone);
        }
    }
}
