using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementToPosition : EnemyMovement {

    //Position variables
    [SerializeField]
    private int maxX, minX, maxY, minY;

    public float initialSpeed = 0.0f;
    public bool moveAfterPosition = false;
    public float postSpeed = 0.0f;
    public float movementLength = 0.0f;

    private bool moveLeft = true;
    private bool inPosition = false;
    Vector3 startPosition, endPosition;

    // Use this for initialization
    void Start ()
    {
        moveLeft = InRightQuadrant();
        InitMovement();
	}
	
	// Update is called once per frame
	protected override void Update ()
    {
        Move();
    }

    void InitMovement()
    {
        inPosition = false;
        startPosition = new Vector3((float)Random.Range(minX, maxX + 1), 12f, 0f);
        endPosition = startPosition;
        endPosition.y = (float)Random.Range(minY, maxY + 1);

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
        if (!inPosition)
        {
            float step = initialSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, step);

            if (transform.position == endPosition) inPosition = true;
        }
        else if (moveAfterPosition)
        {
            inPosition = true;
            Vector3 newPos = this.transform.position;
            float leftBoundary = endPosition.x - (movementLength * 0.5f);
            float rightBoundary = endPosition.x + (movementLength * 0.5f);

            //Find new x position between left and right boundary using speed time time
            if (moveLeft)
            {
                float xpos = Mathf.Clamp(newPos.x + postSpeed * Time.deltaTime, leftBoundary, rightBoundary);
                newPos.x = xpos;
                this.transform.position = newPos;
                if (xpos >= rightBoundary) moveLeft = false;
            }
            else
            {
                float xpos = Mathf.Clamp(newPos.x - postSpeed * Time.deltaTime, leftBoundary, rightBoundary);
                newPos.x = xpos;
                this.transform.position = newPos;
                if (xpos <= leftBoundary) moveLeft = true;
            }
        }
    }

    public override void OnDisable()
    {

    }

    public override void OnEnable()
    {
        moveLeft = InRightQuadrant();
        inPosition = false;
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
