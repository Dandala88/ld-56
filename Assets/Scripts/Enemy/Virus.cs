using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Enemy
{
    public float moveInterval = 5f;
    public float moveForce;

    private float moveElapsed;

    private void Start()
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
    }

    private void Move()
    {
        var rollX = Random.Range(-1f, 1f);
        var rollY = Random.Range(-1f, 1f);
        var rollZ = Random.Range(-1f, 1f);
        var rollVector = new Vector3(rollX, rollY, rollZ);

        rb.AddForce(rollVector * moveForce);
    }
}
