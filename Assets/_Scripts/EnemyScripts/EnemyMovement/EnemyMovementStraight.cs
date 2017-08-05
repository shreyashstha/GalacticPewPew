using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementStraight : EnemyMovement {

    [SerializeField]
    private float speed = 0.0f;
    private bool moveLeft = true;

    // Use this for initialization
	void Start () {
		if (this.transform.position.x > 0)
        {
            moveLeft = true;
        } else if(this.transform.position.x < 0){
            moveLeft = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        if (moveLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    protected override void ToggleSlow()
    {
        throw new NotImplementedException();
    }

    protected override void ToggleFreeze()
    {
        throw new NotImplementedException();
    }
    //*********** EnemyMovement Implementation **********
}
