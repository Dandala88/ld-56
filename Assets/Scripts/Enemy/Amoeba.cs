using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amoeba : Enemy
{
    public float moveInterval = 5f;
    public float shootInterval = 5f;
    public float deceleration;
    public float moveForce;
    public EnemyLaser laserPrefab;
    public bool aggro;

    private float moveElapsed;
    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();
    private Bear bear;

    protected void Awake()
    {
        base.Awake();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach (var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    protected void Start()
    {
        Move();
    }

    private void Update()
    {
        base.Update();
        moveElapsed += Time.deltaTime;

        if (moveElapsed > moveInterval)
        {
            moveElapsed = 0;
            Move();
        }

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
    }

    private void Move()
    {
        if (bear != null)
        {
            rb.AddForce((bear.transform.position - transform.position).normalized * moveForce);
        }
        else
        {
            var rollX = Random.Range(-1f, 1f);
            var rollY = Random.Range(-1f, 1f);
            var rollZ = Random.Range(-1f, 1f);
            var rollVector = new Vector3(rollX, rollY, rollZ);

            rb.AddForce(rollVector * moveForce);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        var bear = other.gameObject.GetComponentInParent<Bear>();
        if (bear != null)
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
