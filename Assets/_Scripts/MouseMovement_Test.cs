using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement_Test : MonoBehaviour {

    float xMin, xMax;
    float shipToEdgePad = 0.5f;
    public float speed = 0.0f;
    
    // Use this for initialization
	void Start () {
        float distance = this.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMostPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMostPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMostPos.x + shipToEdgePad;
        xMax = rightMostPos.x - shipToEdgePad;
    }
	
	// Update is called once per frame
	void Update () {
        HandleInput();
	}

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
    }

    //Player Actions
    void MoveLeft()
    {
        Vector3 pos = this.transform.position;
        //Time.deltaTime makes it frame rate independent - more time to render frame = higher speed, less time to render frame = lower speed
        float newX = Mathf.Clamp(pos.x - speed * Time.deltaTime, xMin, xMax);
        pos.x = newX;
        this.transform.position = pos;
    }

    void MoveRight()
    {
        Vector3 pos = this.transform.position;
        float newX = Mathf.Clamp(pos.x + speed * Time.deltaTime, xMin, xMax);
        pos.x = newX;
        this.transform.position = pos;
    }
}
