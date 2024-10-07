using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAmoeba : Enemy
{
    public float shootInterval = 1f;
    public int shootBurstVolume = 20;
    public float moveSpeed = 50f;
    public float deceleration;
    public EnemyLaser laserPrefab;
    public List<BossWayPoint> wayPoints = new List<BossWayPoint>();

    private float shootElapsed;
    private List<Turret> turrets = new List<Turret>();
    private List<EnemyLaser> laserPool = new List<EnemyLaser>();
    private int laserPoolIterator;
    private int shootBurstIndex;
    private int nextWaypointIndex;

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
        healthbar.transform.position = transform.position + (Vector3.up * transform.localScale.y * 3);

        if (aggro)
        {
            shootElapsed += Time.deltaTime;

            if (shootElapsed > shootInterval)
            {
                shootElapsed = 0;
                StartCoroutine(ShootBurstCoroutine());
            }
        }

        if (Vector3.Distance(rb.position, wayPoints[nextWaypointIndex].transform.position) > 1f)
        {
            rb.position = Vector3.MoveTowards(rb.position, wayPoints[nextWaypointIndex].transform.position, moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            nextWaypointIndex++;
            if (nextWaypointIndex >= wayPoints.Count)
                nextWaypointIndex = 0;
        }
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
