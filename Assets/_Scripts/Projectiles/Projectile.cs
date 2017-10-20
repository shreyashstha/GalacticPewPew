using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour, IProjectile, IPoolableObject {

    // Projectile variables
    public float speed = 20f;    // Projectile Speed
    [SerializeField]
    private int damage = 50;     // Projectile Damage

    // Components
    protected Rigidbody2D _rigidBody;
    protected Collider2D _collider2D;
    protected SpriteRenderer _spriteRenderer;

	void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _collider2D = gameObject.GetComponent<Collider2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    //*****For objectPooling*****
    public virtual void OnEnable()
    {
        //Since Start is only called once in an object's lifetime, we need to call it instead in OnEnable
        Movement();
    }

    public virtual void OnDisable() {}
    //*****Exit ObjectPooling*****

    // Update is called once per frame
    public virtual void Update () {
        if (Boundary.OutOfBoundary(transform.position))
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Hit is called when the projectile hits an enemy ship.
    /// Projectile behaviour can be controlled by this function.
    /// Destroys the projectile from the scene
    /// </summary>
    /// <param name="hitClip">The sound to emit based on what was hit</param>
    public virtual void Hit()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get the damage caused by this projectile
    /// </summary>
    /// <returns>int</returns>
    public int GetDamage()
    {
        return damage;
    }

    public virtual void Movement()
    {
        //rb.velocity = new Vector2(0, speed);
        //rb.AddForce(this.transform.rotation * new Vector2(0, speed * 10));
        _rigidBody.velocity = this.transform.rotation * new Vector2(0f, speed);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Hit();
    }
}
