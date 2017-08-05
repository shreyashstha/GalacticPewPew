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
    // Enemy variables
    public int startHealth = 100;
    //*****Private Variables*****
    private int health = 100;    // Enemy Health
    [SerializeField]
    private int score = 0;		// Score awarded to player on death
    private bool isDead = false;// Status of enemy
    //*****Components*****
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

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
        animator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
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
        boxCollider.enabled = false;
        animator.SetTrigger("death");
        yield return new WaitForSeconds(0.4f);
        //Score stuff 
        GameManager.instance.AddScore(this.score);
        this.transform.parent = null; 
        gameObject.SetActive(false);
    }

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
            spriteRenderer.material.SetFloat("_MaskAmount", amount);
            yield return new WaitForEndOfFrame();
        }

        spriteRenderer.material.SetFloat("_MaskAmount", 0.0f);
    }

    //*****IPoolableObject implementation*****
    public void OnEnable()
    {
        this.health = this.startHealth;
        isDead = false;
        boxCollider.enabled = true;
    }

    public void OnDisable()
    {
    }
}
