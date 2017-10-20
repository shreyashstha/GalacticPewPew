using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ExplodeProjectile : Projectile {

    [SerializeField]
    protected float explosionRadius;      //Radius of the explosion, Circle Collider
    [SerializeField]
    protected float explosionDuration;    //Duration used in coroutine
    [SerializeField]
    private GameObject explosionParticle;   //Particle emitted during hit
    [SerializeField]
    protected AudioClip explodeSound;       //Sound to play when exploding
    private bool firstParticle = true;

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
        float radius = ((CircleCollider2D)_collider2D).radius;
        ((CircleCollider2D)_collider2D).radius = explosionRadius;
        //Wait duration
        yield return new WaitForSeconds(explosionDuration);
        //Revert radius
        ((CircleCollider2D)_collider2D).radius = radius;
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
