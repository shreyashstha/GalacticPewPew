using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private int startHealth = 100;          // Player Starting Health
    //startHealth Property
    public int StartHealth
    {
        get
        {
            return startHealth;
        }
    }

    private int health = 0;                 // Current health
    //health property
    public int Health
    {
        get
        {
            return health;
        }
    }

    public GameObject hitParticle;          // Particle that is instantiated when player is hit

    //Delegate for taking damage
    public delegate void PlayerTakesDamage();
    public PlayerTakesDamage onPlayerTakesDamage;

    
    // Use this for initialization
    void Start () {
        health = startHealth;
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            TakeDamage(projectile.GetDamage());
            //projectile.Hit();
        }
    }

    /// <summary>
    /// Reduce the current health value by damage taken
    /// </summary>
    /// <param name="damage"></param>
    private void TakeDamage(int damage)
    {
        health -= damage;
        EmitParticles();

        if (health <= 0) {
            Die();
        }

        if (onPlayerTakesDamage != null)
        {
            onPlayerTakesDamage();
        }
    }

    private void EmitParticles()
    {
        GameObject newParticle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, 0.3f);
    }

    /// <summary>
    /// take user to the gameover screen and destroy player object.
    /// </summary>
    private void Die()
    {
        GameManager.instance.GameOver();
        gameObject.SetActive(true);
        Destroy(this.gameObject, 2.5f);
    }
}
