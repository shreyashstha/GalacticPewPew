using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PowerUp Abstract Base class
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public abstract class PowerUp : MonoBehaviour {

    private const float DROPSPEED = 5f;

    public Sprite dropSprite;
    [SerializeField]
    protected Vector3 relativePosition = new Vector3(0f, 0.5f, 0f);

    protected SpriteRenderer _spriteRenderer;
    protected Collider2D _collider;
    protected ParticleSystem _dropEmission;
    private bool dropping = true;
    protected bool active = false;

    protected virtual void Start()
    {
        //Get the SpriteRenderer and BoxCollider2D components
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        _dropEmission = gameObject.GetComponent<ParticleSystem>();
        //Set the drop sprite
        _spriteRenderer.sprite = dropSprite;
        //Set isTrigger
        _collider.isTrigger = true;
        _dropEmission.Play();
        gameObject.tag = "Drop";
    }

    protected virtual void Update()
    {
        //Deactivates Power Up if out of boundary
        if (Boundary.OutOfBoundary(transform.position, 0.5f))
        {
            DeactivatePowerUp();
        }
        else if (!active)
        {
            Movement();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //If power up is still falling
        if (dropping)
        {
            //Activate Power Up if in contact with Player collider
            if (other.gameObject.tag == "Player")
            {
                dropping = false;
                active = true;
                ActivatePowerUp();
            }
        }
    }

    protected virtual void Movement()
    {
        transform.Translate(Vector3.down * Time.deltaTime * DROPSPEED);
    }

    /// <summary>
    /// ActivatePowerUp - Called when powerup object touches the player. SetUp function.
    /// </summary>
    public abstract void ActivatePowerUp();
    /// <summary>
    /// ExecutePowerUp - Called to execute the ability of the powerup
    /// </summary>
    public abstract void ExecutePowerUp();
    /// <summary>
    /// DeactivatePowerUp - Called to remove the powerup
    /// </summary>
    public abstract void DeactivatePowerUp();

}
