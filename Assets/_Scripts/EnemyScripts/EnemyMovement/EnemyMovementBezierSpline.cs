using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBezierSpline : EnemyMovement {

    public GameObject splineGO;
    public float duration;          //Time taken to complete the spline.
    private GameObject splineGOCopy;
    private BezierSpline spline;
    private float progress;         //Amount of spline completed.
    private bool moveLeft = true;   //Move left or right based on which quadrant enemy spawned.


    // Use this for initialization
    void Start () {
        ArrangeBezierPoints();
    }
	
	// Update is called once per frame
	protected override void Update () {
        Move();
	}

    void ArrangeBezierPoints()
    {
        moveLeft = InRightQuadrant();
        if (!moveLeft)
        {
            spline.FlipPoints();
        }
    }

    //*********** EnemyMovement Implementation **********
    public override void Move ()
	{
		progress += Time.deltaTime / duration;
		if (progress >= 1f) {
			if (!spline.Loop) {
				progress = 1f;
				gameObject.SetActive (false);
			} else {
				progress -= 1f;
			}
            
        }
        Vector3 position = spline.GetPoint(progress);
        transform.position = position;
    }

    public override void OnDisable()
    {
        //Reset bezier points if it was flipped.
        if (!moveLeft)
        {
            spline.FlipPoints();
        }
    }

    public override void OnEnable()
    {
        if (splineGOCopy == null)
        {
            splineGOCopy = Instantiate(splineGO);
            spline = splineGOCopy.GetComponent<BezierSpline>();
        }
        progress = 0f;
        ArrangeBezierPoints();
    }

    protected override void ToggleFreeze()
    {
        throw new NotImplementedException();
    }

    protected override void ToggleSlow()
    {
        throw new NotImplementedException();
    }
    //*********** EnemyMovement Implementation **********
}
