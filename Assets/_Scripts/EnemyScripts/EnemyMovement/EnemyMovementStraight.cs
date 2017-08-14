using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementStraight : EnemyMovement {

    [SerializeField]
    private float speedX = 0.0f;
    [SerializeField]
    private float speedY = 0.0f;
    private Vector3 startPosition;
    private bool moveLeft = true;
    private float slowDuration = 5.0f;  //TODO: This can be a static variable in game manager or something.
    

    // Use this for initialization
	void Start () {
        startPosition = this.transform.position;
        moveLeft = InRightQuadrant();
    }
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
        Move();
	}

    //*********** EnemyMovement Implementation **********
    public override void Move()
    {
        if (moveLeft)
        {
            transform.Translate(Vector2.left * speedX * Time.deltaTime);
            transform.Translate(Vector2.down * speedY * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.right * speedX * Time.deltaTime);
            transform.Translate(Vector2.down * speedY * Time.deltaTime);
        }
    }

    protected override void ToggleSlow()
    {
        StartCoroutine(ToggleSlowCoroutine());
    }

    IEnumerator ToggleSlowCoroutine()
    {
        speedX = speedX / 2;
        speedY = speedY / 2;
        yield return new WaitForSeconds(slowDuration);
        speedX = speedX * 2;
        speedY = speedY * 2;
    }

    protected override void ToggleFreeze()
    {
        
    }

    public override void OnEnable()
    {
        startPosition = this.transform.position;
        moveLeft = InRightQuadrant();
    }

    public override void OnDisable()
    {
    
    }
    //*********** EnemyMovement Implementation **********
}
