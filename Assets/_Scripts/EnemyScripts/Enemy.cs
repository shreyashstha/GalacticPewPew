using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ObjectPool))]
public class Enemy : MonoBehaviour, IPoolableObject {

    //*****Public Variables*****
    public int startHealth = 100;
    public GameObject hitParticle;
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

    private void Start()
    {
        
    }

    /// <summary>
    /// Reduces health value by damage amount
    /// </summary>
    /// <param name="damage">Amount of health to reduce</param>
    private void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !isDead)
        {
            StartCoroutine(Die());
        } else if (!isDead)
        {
            StartCoroutine("Flash");
        }
    }

    /// <summary>
    /// Performs final duties
    /// </summary>
    IEnumerator Die()
    {
        isDead = true;
        _spriteRenderer.enabled = false;
        _boxCollider.enabled = false;
        EmitParticles(0.5f);
        yield return new WaitForSeconds(0.6f);
        //Score stuff 
        GameManager.instance.AddScore(this.score);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Enemy collider gets triggered when colliding with PlayerProjectile
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Take Damage if hit by a player projectile
        if (other.gameObject.tag == "PlayerProjectile")
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.GetDamage());
            //projectile.Hit();
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

    //*****IPoolableObject implementation*****
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
