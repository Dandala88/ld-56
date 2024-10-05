using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVirus : Enemy
{
    public float shootInterval = 1f;
    public float deceleration;
    public float rotationSpeed;
    public EnemyLaser laserPrefab;

    private float moveElapsed;
    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();
    private Vector3 newRotation;

    protected void Awake()
    {
        base.Awake();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach (var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    private void Update()
    {
        base.Update();

        shootElapsed += Time.deltaTime;

        if (shootElapsed > shootInterval)
        {
            shootElapsed = 0;
            Shoot();
        }

        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        transform.Rotate(0f, rotationSpeed * Time.fixedDeltaTime, 0f);
    }

    private void Shoot()
    {
        foreach (var turret in turrets)
        {
            var clone = Instantiate(laserPrefab);
            clone.transform.position = turret.transform.position;
            clone.transform.forward = turret.transform.forward;
        }
    }
}
