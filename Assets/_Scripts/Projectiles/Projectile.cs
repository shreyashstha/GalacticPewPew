using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour, IProjectile, IPoolableObject {

    // Projectile variables
    public float speed = 20f;    // Projectile Speed
    [SerializeField]
    private int damage = 50;     // Projectile Damage

    // Components
    private Rigidbody2D rb;

	void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        DestroyPorjectileOOB();
	}

    /// <summary>
    /// Destroys the projectile from the scene
    /// </summary>
    public virtual void Hit()
    {
        //TODO: Change this to SetActive
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
        rb.velocity = this.transform.rotation * new Vector2(0f, speed);
    }

    private void DestroyPorjectileOOB()
    {
        if (transform.position.y < Boundary.MinimumY() || transform.position.y > Boundary.MaximumY()
            || transform.position.x < Boundary.MinimumX() || transform.position.x > Boundary.MaximumX())
        {
            Hit();
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Hit();
    }
}
