using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Enemy
{
    public float moveInterval = 5f;
    public float shootInterval = 5f;
    public float deceleration;
    public float moveForce;
    public EnemyLaser laserPrefab;

    private float moveElapsed;
    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();

    protected void Awake()
    {
        base.Awake();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach(var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    protected void Start()
    {
        Move();
    }

    protected void Update()
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
        var rollX = Random.Range(-1f, 1f);
        var rollY = Random.Range(-1f, 1f);
        var rollZ = Random.Range(-1f, 1f);
        var rollVector = new Vector3(rollX, rollY, rollZ);

        rb.AddForce(rollVector * moveForce);
    }

    private void Shoot()
    {
        foreach(var turret in turrets)
        {
            var clone = Instantiate(laserPrefab);
            clone.transform.position = turret.transform.position;
            clone.transform.forward = turret.transform.forward;
        }
    }
}
