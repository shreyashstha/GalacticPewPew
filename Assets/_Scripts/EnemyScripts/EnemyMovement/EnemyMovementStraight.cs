using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementStraight : EnemyMovement {

    [SerializeField]
    private float speed = 0.0f;
    private Vector3 startPosition;
    private bool moveLeft = true;

    // Use this for initialization
	void Start () {
        startPosition = this.transform.position;
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

        // Reset Enemy in the Formation
        if (Mathf.Abs(transform.position.x) > 9f)
        {
            ResetPosition();
        }
	}

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        if (moveLeft)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
    }

    protected override void ToggleSlow()
    {
        //throw new NotImplementedException();
    }

    protected override void ToggleFreeze()
    {
        //throw new NotImplementedException();
    }

    protected override void ResetPosition()
    {
        Vector3 newPos = startPosition;
        float newY = (float)Random.Range(3, 8);
        newPos.y = newY;
        this.transform.position = newPos;
    }
    //*********** EnemyMovement Implementation **********
}
