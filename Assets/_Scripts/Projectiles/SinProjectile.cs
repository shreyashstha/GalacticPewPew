﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinProjectile : Projectile {

    //*****Public variables*****
    public float amplitude = 0.0f;
    public float frequency = 1.0f;

    //*****Private variables*****
    private Vector3 startPosition;
    private bool initial = true;    //For Setting up intial position. Since object pooling, position is set after enabling, need to get start position in update once.
    private float angle = 0.0f;

    public override void OnEnable()
    {
        angle = 0f;
        startPosition = this.transform.position;
    }

    // Use this for initialization
    public override void Update () {
        
        base.Update();
        Movement();
    }

    public override void Movement()
    {
        Vector3 newPos = this.transform.position;
        Debug.Log(this.transform.position + "...angle..." + angle);
        newPos.x = startPosition.x + (amplitude * Mathf.Sin(angle * frequency));
        newPos.y = newPos.y - speed * Time.deltaTime;
        this.transform.position = newPos;
        angle += Time.deltaTime;
    }
}
