using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementToPosition : EnemyMovement {

    //Position variables
    [SerializeField]
    private int maxX, minX, maxY, minY;     //A box within which the gameobject's initial position can be within

    public float initialSpeed = 0.0f;       //How fast the gameobject gets to the initial position
    public bool moveAfterPosition = false;  //Determines if gameobject will move after initial position
    public bool moveRandAfterPosition = false;             //Randomizes secondary movement
    public float postSpeed = 0.0f;          //Speed of secondary movement
    public float movementLength = 0.0f;     //Length of secondary movement
    private bool moveLeft = true;           //Determines if gameobject will move left or right initially
    private bool inPosition = false;        //Determines if gameobject is in the initial position
    private bool inRandomPosition = true;  //Determines if gameobject is in random position.
    Vector3 startPosition, initialPosition, randomPosition;     //starting position outside screen, ending position within box
                                                                //Randomposition is used if random is selected

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

    /// <summary>
    /// Initializes the game object for movement.
    /// -Randomly determines startPosition and initialPosition.
    /// -Flips vectors if initially moving left
    /// -Moves gameobject to start position
    /// </summary>
    void InitMovement()
    {
        inPosition = false;
        startPosition = new Vector3((float)Random.Range(minX, maxX + 1), 12f, 0f);
        initialPosition = startPosition;
        initialPosition.y = (float)Random.Range(minY, maxY + 1);

        if (moveLeft)
        {
            startPosition = Vector3.Scale(startPosition, new Vector3(-1f, 1f, 1f));
            initialPosition = Vector3.Scale(initialPosition, new Vector3(-1f, 1f, 1f));
        }

        transform.position = startPosition;
    }

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        if (!inPosition)
        {
            float step = initialSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

            if (transform.position == initialPosition) inPosition = true;
        }
        else if (moveAfterPosition && inPosition)
        {
            MoveAfterPosition();
        }
        else if (moveRandAfterPosition && inPosition)
        {
            MoveRandomAfterPosition();
        }
    }

    private void MoveAfterPosition()
    {
        Vector3 newPos = this.transform.position;
        float leftBoundary = initialPosition.x - (movementLength * 0.5f);
        float rightBoundary = initialPosition.x + (movementLength * 0.5f);

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

    private void MoveRandomAfterPosition()
    {
        if (inRandomPosition)
        {
            inRandomPosition = false;
            randomPosition = transform.position;
            randomPosition.x = (float)Random.Range(minX, maxX * 2 + 1) - maxX;
            randomPosition.y = (float)Random.Range(minY, maxY + 1);
        }
        
        float step = postSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, randomPosition, step);

        if (transform.position == randomPosition) inRandomPosition = true;
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
