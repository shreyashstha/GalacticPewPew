using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementWave : EnemyMovement {

    //*****Private variables*****
    [SerializeField]
    private float amplitude = 0.0f;
    [SerializeField]
    private float frequency = 1.0f;
    [SerializeField]
    private float speed = 0.0f;
    private Vector3 startPosition;
    private float angle = 0.0f;
    private bool moveLeft = true;
    [SerializeField]
    private bool moveSin = true;

    // Use this for initialization
    void Start () {
        startPosition = this.transform.position;
        moveLeft = InRightQuadrant();
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();
        Move();
    }

    //*********** EnemyMovement Implementation **********

    public override void Move()
    {
        if (moveSin)
        {
            MoveSin();
        } else
        {
            MoveCos();
        }
        
        
    }

    public override void OnDisable()
    {
        //Nothing to do
    }

    public override void OnEnable()
    {
        angle = 0f;
        startPosition = this.transform.position;
        moveLeft = InRightQuadrant();
    }

    protected override void ToggleFreeze()
    {
        //TODO: When implementing powerups
    }

    protected override void ToggleSlow()
    {
        
    }
    //*********** EnemyMovement Implementation **********

    private void MoveSin()
    {
        Vector3 newPos = this.transform.position;
        if (moveLeft)
        {
            //Calculate new x and y positons.
            newPos.y = startPosition.y + (amplitude * Mathf.Sin(angle * frequency));
            newPos.x = newPos.x - speed * Time.deltaTime;
            //Rotate if possible
            if (canRotate) ChangeRotation(newPos);
            //Set new rotation
            this.transform.position = newPos;
            //Increase angle for next iteration.
            angle += Time.deltaTime;
        }
        else
        {
            //Calculate new x and y positons.
            newPos.y = startPosition.y + (amplitude * Mathf.Sin(angle * frequency));
            newPos.x = newPos.x + speed * Time.deltaTime;
            //Rotate if possible
            if (canRotate) ChangeRotation(newPos);
            //Set new rotation
            this.transform.position = newPos;
            //Increase angle for next iteration.
            angle += Time.deltaTime;
        }
    }
    private void MoveCos()
    {
        Vector3 newPos = this.transform.position;
        if (moveLeft)
        {
            //Calculate new x and y positons.
            newPos.y = startPosition.y + (amplitude * Mathf.Cos(angle * frequency));
            newPos.x = newPos.x - speed * Time.deltaTime;
            //Rotate if possible
            if (canRotate) ChangeRotation(newPos);
            //Set new rotation
            this.transform.position = newPos;
            //Increase angle for next iteration.
            angle += Time.deltaTime;
        }
        else
        {
            //Calculate new x and y positons.
            newPos.y = startPosition.y + (amplitude * Mathf.Cos(angle * frequency));
            newPos.x = newPos.x + speed * Time.deltaTime;
            //Rotate if possible
            if (canRotate) ChangeRotation(newPos);
            //Set new rotation
            this.transform.position = newPos;
            //Increase angle for next iteration.
            angle += Time.deltaTime;
        }
    }
}
