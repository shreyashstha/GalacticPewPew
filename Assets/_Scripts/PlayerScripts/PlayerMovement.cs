using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // Player Variables
    public float speed = 0.45f;         // Player Speed

    // Camera Variables
    public float shipEdgePad = 0.5f;       // Pads ship on the edge so that parts of the ship is not off the screen
    private float minXPos = 0.0f;          // Minimum x position the ship can move to calculated by ViewportToWorldPoint
    private float maxXPos = 0.0f;          // Maximum x position the ship can move to calculated by ViewportToWorldPoint

    // Components
    Animator anim;

    private void Awake()
    {
        // Get Animator Component
        anim = this.GetComponent<Animator>();
    }

    // Use this for initialization
    void Start ()
    {
        // Calculate x boundaries
        minXPos = Boundary.MinimumX() + shipEdgePad;
        maxXPos = Boundary.MaximumX() - shipEdgePad;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!GameManager.instance.Paused)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        /**this.transform.position = new Vector3(mousePosInBlocks, this.transform.position.y, this.transform.position.z);
        mousePosInBlocks = Mathf.Clamp(((Input.mousePosition.x / Screen.width) * 8) - 4, minXPos, maxXPos);
        print(mousePosInBlocks);
        **/
        
        if (Input.touchCount == 1)
        {
            // If screen was touched and touch phase is Moved move the ship to the new position
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MovePlayer(Input.GetTouch(0));
            }
            // Reset animation
            else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                ResetAnimation();
            }
        }  
    }

    private void MovePlayer(Touch touch)
    {
        // Save current ship position
        Vector3 newShipPosition = this.transform.position;

        // Get new x position by transforming Screen point to World point
        Vector3 screenToWorldVector = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, this.transform.position.z - Camera.main.transform.position.z));
        if(screenToWorldVector.y < -9f)
        {
            newShipPosition.x = Mathf.Clamp(screenToWorldVector.x, minXPos, maxXPos);
        }

        // Change Animation states to represent movement
        if (transform.position.x > newShipPosition.x)
        {
            anim.SetBool("MovingRight", false);
            anim.SetBool("MovingLeft", true);
        }
        else if (transform.position.x < newShipPosition.x)
        {
            anim.SetBool("MovingLeft", false);
            anim.SetBool("MovingRight", true);
        }
        // Lerp to new position
        transform.position = Vector3.Lerp(transform.position, newShipPosition, speed);
    }

    private void ResetAnimation()
    {
        // Reset Animation state to idle
        anim.SetBool("MovingLeft", false);
        anim.SetBool("MovingRight", false);
    }
}
