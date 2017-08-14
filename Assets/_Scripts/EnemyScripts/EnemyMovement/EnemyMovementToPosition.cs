using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementToPosition : EnemyMovement {

    Vector3 startPosition, endPosition;
    public float moveSpeed = 0.0f;
    private bool moveLeft = true;

    // Use this for initialization
    void Start ()
    {
        moveLeft = InRightQuadrant();
        InitMovement();
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        Move();
    }

    void InitMovement()
    {
        startPosition = new Vector3((float)Random.Range(1, 6), 12f, 0f);
        endPosition = startPosition;
        endPosition.y = (float)Random.Range(1,4);

        if (moveLeft)
        {
            startPosition = Vector3.Scale(startPosition, new Vector3(-1f, 1f, 1f));
            endPosition = Vector3.Scale(endPosition, new Vector3(-1f, 1f, 1f));
        }

        transform.position = startPosition;
    }

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        moveLeft = InRightQuadrant();
        InitMovement();
    }

    protected override void ToggleFreeze()
    {

    }

    protected override void ToggleSlow()
    {

    }
    //*********** EnemyMovement Implementation **********
}
