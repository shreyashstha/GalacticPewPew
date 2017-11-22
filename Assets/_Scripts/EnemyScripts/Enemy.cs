using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Name: Enemy
/// Description: Enemy base class.
/// - handling enemy health
/// - getting components
/// - colliding with projectiles
/// - disabling
/// - dropping player health piece
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ObjectPool))]
public class Enemy : MonoBehaviour, IPoolableObject {

    //*****Public Variables*****
    public int startHealth = 100;
    public GameObject hitParticle;  //Explosion particl
    public GameObject dropObject;   //TODO: Object dropped that user picks up to get heal.
    public AudioClip hitClip;       //sound to play when hit by player projectile
    public AudioClip deathClip;     //sound to play when being destroyed
    //*****Private Variables*****
    private int health = 100;    // Enemy Health
    [SerializeField]
    private int score = 0;		// Score awarded to player on death
    private bool isDead = false;// Status of enemy
    //*****Components*****
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;

    //isDead property
    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Use this for initialization
    void Awake () {
        _animator = this.gameObject.GetComponent<Animator>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        _boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
	}

    /// <summary>
    /// Reduces health value by damage amount.
    /// Call coroutine Die if health is 0.
    /// </summary>
    /// <param name="damage">Amount of health to reduce</param>
    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            StartCoroutine(Die(this.score));
        } else if (!isDead)
        {
            StartCoroutine("Flash");
        }
    }

    /// <summary>
    /// Performs final duties:
    /// Disables sprite renderer, box collider
    /// Gamemanager - adds score, increments kill count
    /// </summary>
    IEnumerator Die(int scoreToAdd)
    {
        isDead = true;
        _spriteRenderer.enabled = false;
        _boxCollider.enabled = false;
        AudioSource.PlayClipAtPoint(deathClip, transform.position);
        GameManager.instance.AddScore(scoreToAdd);
        GameManager.instance.IncrementEnemyKills();
        EmitParticles(0.5f);
        DropHealth();
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Enemy collider gets triggered when colliding with PlayerProjectile and Asteroid
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Take Damage if hit by a player projectile
        if (other.gameObject.tag == "PlayerProjectile")
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position);
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.GetDamage());
        }else if (other.gameObject.tag == "Asteroid")
        {
            //Kill player without adding score.
            StartCoroutine(Die(0));
        }
    }

    /// <summary>
    /// Creates a Flash effect
    /// </summary>
    /// <returns></returns>
    IEnumerator Flash()
    {
        float min = 0.0f;
        float max = 1.0f;
        float t = 0.0f;

        while (t < 1.0f)
        {
            t += 0.15f;

            float amount = Mathf.Lerp(min, max, t);
            _spriteRenderer.material.SetFloat("_MaskAmount", amount);
            yield return new WaitForEndOfFrame();
        }

        _spriteRenderer.material.SetFloat("_MaskAmount", 0.0f);
    }

    /// <summary>
    /// Creates a particle emitter and destroys it after 0.3 secs
    /// </summary>
    private void EmitParticles(float time)
    {
        GameObject newParticle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, time);
    }

    /// <summary>
    /// Determines if item needs to be dropped for player
    /// </summary>
    private void DropHealth()
    {
        int random = UnityEngine.Random.Range(0, 99);
        if (random < 5)
        {
            Instantiate(dropObject, transform.position, Quaternion.identity);
        }
    }

    //*****IPoolableObject implementation*****
    /// <summary>
    /// Reset health
    /// enabling components
    /// </summary>
    public void OnEnable()
    {
        this.health = this.startHealth;
        isDead = false;
        _boxCollider.enabled = true;
        _spriteRenderer.enabled = true;
    }

    public void OnDisable(){}
    //*****End Object pool implementation*****
}
