using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyMovement : MonoBehaviour, IMovement {

    private bool canMove = true;

    /// <summary>
    /// Function to toggle Movement. A way to stop movement. Useful for effects like freeze
    /// </summary>
    protected virtual void ToggleCanMove()
    {
        canMove = !canMove;
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

    /// <summary>
    /// Reset position when enemyship is out of bounds or any other reason.
    /// </summary>
    protected abstract void ResetPosition();
}
