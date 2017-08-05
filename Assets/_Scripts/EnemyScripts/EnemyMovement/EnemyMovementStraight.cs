using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementStraight : EnemyMovement {

    [SerializeField]
    private float speed = 0.0f;
    private Vector3 startPosition;
    private bool moveLeft = true;
    private float slowDuration = 5.0f;
    

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
        if (Mathf.Abs(transform.position.x) > 15f)
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
        
    }

    IEnumerator ToggleSlowCoroutine()
    {
        speed = speed / 2;
        yield return new WaitForSeconds(slowDuration);
        speed = speed * 2;
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
        this.transform.position = startPosition;
    }

    public override void OnEnable()
    {
        startPosition = this.transform.position;
        if (this.transform.position.x > 0)
        {
            moveLeft = true;
        }
        else if (this.transform.position.x < 0)
        {
            moveLeft = false;
        }
    }

    public override void OnDisable()
    {
        //throw new NotImplementedException();
    }
    //*********** EnemyMovement Implementation **********
}
