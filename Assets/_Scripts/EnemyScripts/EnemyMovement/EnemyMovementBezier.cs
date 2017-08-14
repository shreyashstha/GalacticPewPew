using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyMovementBezier : EnemyMovement {

    // each 4 points are used to create Bezier curves.
    //Ideally the end point and start point of consecutive curves should be the same.
    //The second control point of the one curve and the first control point of the consecutive curve should be mirrored for smoothness.
    public Vector3[] bezierPoints;
    public int curves = 0;
    public int steps = 0;
    private bool moveLeft = true;

    private Vector3[] bezierPositions;
    private int bezierIndex = 0;

    // Use this for initialization
    void Start () {
        ArraneBezierPoints();
        if (bezierPoints.Length == 0) Debug.Log("I should throw an error");
        bezierPositions = GetBezierPositions();
        this.transform.position = bezierPositions[bezierIndex];
        bezierIndex++;
	}
	
	// Update is called once per frame
	public override void Update () {
        Move();
	}

    void ArraneBezierPoints()
    {
        moveLeft = InRightQuadrant();
        if (!moveLeft)
        {
            for (int i = 0; i < bezierPoints.Length; i++)
            {
                bezierPoints[i] = Vector3.Scale(bezierPoints[i], new Vector3(-1f, 1f, 1f));
            }
        }
    }

    Vector3[] GetBezierPositions()
    {
        Vector3[] returnArray = new Vector3[curves * steps];

        if (bezierPoints.Length % 4 != 0) Debug.Log("Incorrect number of points for Bezier curves");

        for (int i = 0; i < bezierPoints.Length; i += 4)
        {
            Vector3[] positions = BezierPostions(bezierPoints[i], bezierPoints[i + 1], bezierPoints[i + 2], bezierPoints[i + 3], steps);
            Array.Copy(positions, 0, returnArray, (i/4) * steps, positions.Length);
        }
        return returnArray;
    }

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {

        Vector3 newPos = bezierPositions[bezierIndex];
        if (canRotate)
        {
            ChangeRotation(newPos);
        }
        this.transform.position = newPos;
        bezierIndex++;
        if (bezierIndex == bezierPositions.Length)
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnDisable()
    {
        
    }

    public override void OnEnable()
    {
        ArraneBezierPoints();
        bezierIndex = 1;
        bezierPositions = GetBezierPositions();
        this.transform.position = bezierPositions[0];
    }

    protected override void ToggleFreeze()
    {
        //throw new NotImplementedException();
    }

    protected override void ToggleSlow()
    {
        //throw new NotImplementedException();
    }
    //*********** EnemyMovement Implementation **********

    /// <summary>
    /// Given 4 Vector3, two control points, a start point and end point
    /// </summary>
    /// <param name="p0">Start Point</param>
    /// <param name="p1">First Control Point</param>
    /// <param name="p2">Second Control Point</param>
    /// <param name="p3">End Point</param>
    /// <param name="steps">Number of linear interpolations - Makes curve smoother</param>
    /// <returns></returns>
    Vector3[] BezierPostions(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int steps)
    {
        Vector3[] points = new Vector3[steps];

        for (int i = 0; i < steps; i++)
        {
            Vector3 point = BezierPoint(p0, p1, p2, p3, ((float)i / (float)steps));
            points[i] = point;
        }

        return points;
    }

    /// <summary>
    /// Interpolates between Bezier curve at t
    /// </summary>
    /// <param name="p0">Start Point</param>
    /// <param name="p1">First Control Point</param>
    /// <param name="p2">Second Control Point</param>
    /// <param name="p3">End Point</param>
    /// <param name="t">t value [0,1] percent interpolation</param>
    /// <returns></returns>
    Vector3 BezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float mt3 = (1 - t) * (1 - t) * (1 - t);
        float mt2 = (1 - t) * (1 - t);
        float t2 = t * t;
        float t3 = t * t * t;

        return (p0 * mt3) + (3 * p1 * mt2 * t) + (3 * p2 * (1 - t) * t2) + (p3 * t3);
    }
}
