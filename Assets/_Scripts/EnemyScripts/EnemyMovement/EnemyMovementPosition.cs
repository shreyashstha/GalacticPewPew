using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementPosition : EnemyMovement {

    //***** Private Variables *****
    [SerializeField]
    private float moveSpeed = 0.0f;     //The movement speed of this ship
    [SerializeField]
    private int xPositionMax = 0;
    [SerializeField]
    private int xPositionMin = 0;
    [SerializeField]
    private int yPositionMax = 0;
    [SerializeField]
    private int yPositionMin = 0;

    private Vector3 startPosition;      //Which x position to start InitializeMovement()
    private Vector3 endPosition;        //Which y position to end InitializeMovement()
    private bool moveLeft = true;       //True if transform is on the left quadrant on the game screed. Start and end positions are dependent on this
    private bool inPosition = true;    //True if transform is in the end position. Reset (false) when initilized.
    private float slowDuration = 5.0f;  //TODO: This can be a static variable in game manager or something.


    // Update is called once per frame
    public override void Update()
    {
        while (!inPosition)
        {
            //Move();
        }
    }

    void InitializeMovement()
    {
        inPosition = false;
        moveLeft = InRightQuadrant();
        if (moveLeft)
        {
            startPosition = new Vector3((float)Random.Range(xPositionMin, xPositionMax), 11f);
            endPosition = startPosition;
            endPosition.y = (float)Random.Range(yPositionMin, yPositionMax + 1);
        }
        else
        {
            startPosition = new Vector3(-1f * (float)Random.Range(xPositionMin, xPositionMax), 11f);
            endPosition = startPosition;
            endPosition.y = (float)Random.Range(xPositionMin, xPositionMax + 1);
        }
        transform.position = startPosition;
        moveSpeed = Vector3.Distance(transform.position, endPosition);
    }

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        Debug.Log("yo");
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPosition, step);
        if (transform.position == endPosition) inPosition = true;
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        InitializeMovement();
    }

    protected override void ToggleFreeze()
    {
        
    }

    protected override void ToggleSlow()
    {
        
    }
    //*********** EnemyMovement Implementation **********

    IEnumerator ToggleSlowCoroutine()
    {
        moveSpeed = moveSpeed / 2;
        yield return new WaitForSeconds(slowDuration);
        moveSpeed = moveSpeed * 2;
    }
}
