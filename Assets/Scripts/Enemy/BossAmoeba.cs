using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAmoeba : Enemy
{
    public float shootInterval = 1f;
    public int shootBurstVolume = 20;
    public float radius = 30f;
    public float deceleration;
    public float rotationSpeed;
    public EnemyLaser laserPrefab;

    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();
    private List<EnemyLaser> laserPool = new List<EnemyLaser>();
    private int laserPoolIterator;
    private int shootBurstIndex;

    protected void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        healthbar.enabled = false;
        SetupLaserPool();
        var turretChildren = GetComponentsInChildren<Turret>();
        foreach (var turretChild in turretChildren)
            turrets.Add(turretChild);
    }

    protected void Update()
    {
        base.Update();

        if (aggro)
        {
            shootElapsed += Time.deltaTime;

            if (shootElapsed > shootInterval)
            {
                shootElapsed = 0;
                StartCoroutine(ShootBurstCoroutine());
            }
        }

        rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        transform.Rotate(rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime, rotationSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator ShootBurstCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Shoot();
        shootBurstIndex++;
        if(shootBurstIndex < shootBurstVolume)
            StartCoroutine(ShootBurstCoroutine());
        else
            shootBurstIndex = 0;
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
                clone.transform.forward = (bear.transform.position - turret.transform.position).normalized;
            else
                clone.transform.forward = turret.transform.forward;
            clone.gameObject.SetActive(true);
        }
    }

    private void SetupLaserPool()
    {
        for (int i = 0; i < 1000; i++)
        {
            var clone = Instantiate(laserPrefab);
            clone.gameObject.SetActive(false);
            laserPool.Add(clone);
        }
    }
}
