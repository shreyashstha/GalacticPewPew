using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyMovement : MonoBehaviour, IMovement, IPoolableObject {


    private bool canMove = true;
    [SerializeField]
    protected bool canRotate = true;

    // Update is called once per frame
    public virtual void Update()
    {
        DisableShipOOB();
    }

    /// <summary>
    /// Function to toggle Movement. A way to stop movement. Useful for effects like freeze
    /// </summary>
    protected virtual void ToggleCanMove()
    {
        canMove = !canMove;
    }

    /// <summary>
    /// Rotates the gameobject to face target direction. Sprites are facing down so 90 degrees are added to the new rotation.
    /// </summary>
    /// <param name="targetPosition"></param>
    protected virtual void ChangeRotation(Vector3 targetPosition)
    {
        Vector2 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    /// <summary>
    /// Returns true if located in the right quadrant of the game screen, false othewise
    /// </summary>
    /// <returns>boolean</returns>
    protected bool InRightQuadrant()
    {
        if (this.transform.position.x > 0)
        {
            return true;
        }
        else if (this.transform.position.x < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Disable the Enemy gameobject when out of bounds. This is so that we don't overflood the game with enemies and they are removed when not killed.
    /// </summary>
    private void DisableShipOOB()
    {
        if (transform.position.y < Boundary.MinimumY() - 3f || transform.position.y > Boundary.MaximumY() + 3f
            || transform.position.x < Boundary.MinimumX() - 3f || transform.position.x > Boundary.MaximumX() + 3f)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// OnTriggerEnter2D to detect status effect stuff like poison or Freeze.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Status effect tags
        if (other.gameObject.tag == "SlowEffect")
        {
            Debug.Log("Hey");
            ToggleSlow();
        } else if (other.gameObject.tag == "FreezeEffect")
        {
            ToggleFreeze();
        }
    }

    /// <summary>
    /// Handles how this gameobject moves.
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// Enemies are susceptible to slow effects. Toggle slow effect for the movement.
    /// Called by OnTriggerEnter2D in base class
    /// </summary>
    protected abstract void ToggleSlow();

    /// <summary>
    /// Enemies are susceptible to freeze effects. Toggle slow effect for the movement.
    /// Called by OnTriggerEnter2D in base class
    /// </summary>
    protected abstract void ToggleFreeze();

    public abstract void OnEnable();

    public abstract void OnDisable();
}
