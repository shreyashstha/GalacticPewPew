using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
    leftandright,
    upanddown,
    circle,
    bezier,
    nomovement
}


public class EnemyPosition : MonoBehaviour {

    private Vector3 startPosition = new Vector3(0f, 0f, 0f);
    private bool isInStartPosition = true;
    private MovementType movementType;

    private float speed = 2.0f;
    private float angularSpeed = 50f;
    private const float moveToStartosTime = 2.0f;
    private float moveToStartPosSpeed = 0.0f;

    //Variables for MoveLeftAndRight
    private bool moveLeft = true;
    private float leftBoundary = 0.0f;
    private float rightBoundary = 0.0f;
    //Variables for MoveUpAndDown
    private bool moveUp = true;
    private float topBoundary = 0.0f;
    private float bottomBoundary = 0.0f;
    //Variables for Circle
    private Vector3 center;
    private bool clockwise = true;
    //Variables for Bezier Curve
    private Vector3[] bezierPoints;
    private int currentBezierIndex;
    private int nextBezierIndex;
    private int interpSteps;
    private int interpT = 0;


    public void ChangeMovement (Vector3 startPosition, MovementType type, float distance, bool move = true)
    {
        this.startPosition = startPosition;
        this.moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition);
        isInStartPosition = false;
        this.movementType = type;
        if (type == MovementType.leftandright)
        {
            this.moveLeft = move;
            this.leftBoundary = startPosition.x - distance * 0.5f;
            this.rightBoundary = startPosition.x + distance * 0.5f;
        } else if (type == MovementType.upanddown)
        {
            this.moveUp = move;
            this.topBoundary = startPosition.y + distance * 0.5f;
            this.bottomBoundary = startPosition.y - distance * 0.5f;
        }
    }

    public void ChangeMovement(Vector3 startPosition, float midDimention, MovementType type, float distance, bool moveLeftOfUp = true)
    {
        this.startPosition = startPosition;
        this.moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition);
        isInStartPosition = false;
        this.movementType = type;
        if (type == MovementType.leftandright)
        {
            this.moveLeft = moveLeftOfUp;
            this.leftBoundary = midDimention - distance * 0.5f;
            this.rightBoundary = midDimention + distance * 0.5f;
        }
        else if (type == MovementType.upanddown)
        {
            this.moveUp = moveLeftOfUp;
            this.topBoundary = midDimention + distance * 0.5f;
            this.bottomBoundary = midDimention - distance * 0.5f;
        }
    }

    public void ChangeMovement (Vector3 startPosition, Vector3 center, bool clockwise = true)
    {
        this.startPosition = startPosition;
        this.moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition);
        this.isInStartPosition = false;
        this.movementType = MovementType.circle;
        this.center = center;
        this.clockwise = clockwise;
    }

    public void ChangeMovement(Vector3[] bezierPoints, int positionInCurve)
    {
        this.movementType = MovementType.bezier;

        if (bezierPoints.Length == 0)
        {
            Debug.Log("Empty Exception while changing movement");
        }

        this.bezierPoints = bezierPoints;

        if (positionInCurve < bezierPoints.Length)
        {
            this.startPosition = bezierPoints[positionInCurve];
            this.moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition);
            this.isInStartPosition = false;
            this.currentBezierIndex = positionInCurve;
        } else
        {
            this.startPosition = bezierPoints[0];
            this.moveToStartPosSpeed = Vector3.Distance(transform.position, startPosition);
            this.isInStartPosition = false;
            this.currentBezierIndex = 0;
        }
        if (positionInCurve == bezierPoints.Length - 1)
        {
            this.nextBezierIndex = 0;
        }
        else
        {
            this.nextBezierIndex = positionInCurve + 1;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isInStartPosition)
        {
            MoveToStartPosition();
        }
        else
        {
            switch (movementType)
            {
                case MovementType.nomovement:
                    break;
                case MovementType.leftandright:
                    MoveLeftAndRight();
                    break;
                case MovementType.upanddown:
                    MoveUpAndDown();
                    break;
                case MovementType.circle:
                    MoveInACircle();
                    break;
                case MovementType.bezier:
                    MoveThroughBezierCurve();
                    break;
            }
        }
    }

    void MoveToStartPosition()
    {
        float step = this.moveToStartPosSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, startPosition, step);
        if (transform.position == startPosition) isInStartPosition = true;
    }

    void MoveLeftAndRight()
    {
        Vector3 newPos = this.transform.position;
        //Find new x position between left and right boundary using speed time time
        if (moveLeft)
        {
            float xpos = Mathf.Clamp(newPos.x - speed * Time.deltaTime, leftBoundary, rightBoundary);
            newPos.x = xpos;
            this.transform.position = newPos;
            if (xpos <= leftBoundary) moveLeft = false;
        }
        else
        {
            float xpos = Mathf.Clamp(newPos.x + speed * Time.deltaTime, leftBoundary, rightBoundary);
            newPos.x = xpos;
            this.transform.position = newPos;
            if (xpos >= rightBoundary) moveLeft = true;
        }
    }

    void MoveUpAndDown()
    {
        Vector3 newPos = this.transform.position;

        if (moveUp)
        {
            float ypos = Mathf.Clamp(newPos.y + speed * Time.deltaTime, bottomBoundary, topBoundary);
            newPos.y = ypos;
            this.transform.position = newPos;
            if (ypos >= topBoundary) moveUp = false;
        }
        else
        {
            float ypos = Mathf.Clamp(newPos.y - speed * Time.deltaTime, bottomBoundary, topBoundary);
            newPos.y = ypos;
            this.transform.position = newPos;
            if (ypos <= bottomBoundary) moveUp = true;
        }
    }

    void MoveInACircle ()
    {
        Vector3 newPos = this.transform.position - this.center;
        if (clockwise)
        {
            newPos = Quaternion.AngleAxis(-angularSpeed * Time.deltaTime, Vector3.forward) * newPos;
        }else
        {
            newPos = Quaternion.AngleAxis(angularSpeed * Time.deltaTime, Vector3.forward) * newPos;
        }
        this.transform.position = this.center + newPos;
    }

    void MoveThroughBezierCurve() {
        Vector3 newPos = bezierPoints[nextBezierIndex];
            //Vector3.Lerp(bezierPoints[currentBezierIndex], bezierPoints[nextBezierIndex], ((float)interpT/(float)interpSteps));
        currentBezierIndex = nextBezierIndex;
        if (nextBezierIndex == bezierPoints.Length - 1)
        {
            nextBezierIndex = 0;
        }
        else
        {
            nextBezierIndex = nextBezierIndex + 1;
        }
        this.transform.position = newPos;
    }
}


