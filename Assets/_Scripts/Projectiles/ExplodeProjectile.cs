using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeProjectile : Projectile {

    [SerializeField]
    protected float explosionRadius;      //Radius of the explosion, Circle Collider
    [SerializeField]
    protected float explosionDuration;    //Duration used in coroutine
    [SerializeField]
    private GameObject explosionParticle;   //Particle emitted during hit
    private bool firstParticle = true;

    public override void Update()
    {
        if (Boundary.OutOfBoundary(transform.position))
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Override:
    /// Enabling SpriteRenderer
    /// Setting bool firstParticle to true
    /// </summary>
    public override void OnEnable()
    {
        _spriteRenderer.enabled = true;
        firstParticle = true;
        base.OnEnable();
    }

    /// <summary>
    /// Override:
    /// Disable Spriterendere when hit
    /// Stop Movement
    /// EmitParticles
    /// Start Explosion coroutine
    /// </summary>
    public override void Hit()
    {
        _spriteRenderer.enabled = false;
        StopMovement();
        if (firstParticle)
        {
            EmitParticles(explosionDuration);
            firstParticle = false;
        }
        StartCoroutine(ExplodeCoroutine());
    }

    /// <summary>
    /// Coroutine that creates the explosion effect.
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator ExplodeCoroutine()
    {
        //Store original radius
        float radius = _circleCollider2D.radius;
        _circleCollider2D.radius = explosionRadius;
        //Wait duration
        yield return new WaitForSeconds(explosionDuration);
        //Revert radius
        _circleCollider2D.radius = radius;
        this.gameObject.SetActive(false);
    }

    void StopMovement()
    {
        _rigidBody.velocity = this.transform.rotation * new Vector2(0f, 0f);
    }

    /// <summary>
    /// Creates a particle emitter and destroys it after 0.3 secs
    /// </summary>
    private void EmitParticles(float time)
    {
        GameObject newParticle = Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, time);
    }
}
