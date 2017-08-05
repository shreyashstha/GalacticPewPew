﻿using System.Collections;
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

    // Use this for initialization
    void Start () {
        startPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Move();

        // Reset Enemy in the Formation
        if (Mathf.Abs(transform.position.y) > 15f)
        {
            ResetPosition();
        }
    }

    //*********** EnemyMovement Implementation **********

    public override void Move()
    {
        Vector3 newPos = this.transform.position;
        newPos.x = startPosition.x + (amplitude * Mathf.Sin(angle * frequency));
        newPos.y = newPos.y - speed * Time.deltaTime;
        this.transform.position = newPos;
        angle += Time.deltaTime;
    }

    public override void OnDisable()
    {
        //Nothing to do
    }

    public override void OnEnable()
    {
        angle = 0f;
        startPosition = this.transform.position;
    }

    protected override void ResetPosition()
    {
        transform.position = startPosition;
    }

    protected override void ToggleFreeze()
    {

    }

    protected override void ToggleSlow()
    {
        
    }
    //*********** EnemyMovement Implementation **********
}
