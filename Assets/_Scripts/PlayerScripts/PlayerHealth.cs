using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private int startHealth = 40;          //Player Starting Health
    private int health = 0;                 //Current health
    private PlayerScript _playerScript;     //PlayerScript Component
    [SerializeField]
    private AudioClip hitClip;      //AudioClip to play when hit

    public int StartHealth      //startHealth Property
    {
        get
        {
            return startHealth;
        }
    }
    public int Health           //health property
    {
        get
        {
            return health;
        }
    }

    public GameObject hitParticle;          // Particle that is instantiated when player is hit

    //Delegate when taking damage
    public delegate void PlayerTakesDamage();
    public PlayerTakesDamage onPlayerTakesDamage;
    //Delegate when adding health
    public delegate void PlayerAddHealth();
    public PlayerAddHealth onPlayerAddHealth;

    private void Awake()
    {
        _playerScript = gameObject.GetComponent<PlayerScript>();
    }

    // Use this for initialization
    void Start () {
        health = startHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            TakeDamage(projectile.GetDamage());
            //projectile.Hit();
        }if (collision.gameObject.tag == "HealthPiece")
        {
            GameManager.instance.IncrementHealthPiece();
            Destroy(collision.gameObject);
        }
    }

    public void AddHealth()
    {
        this.health = Mathf.Clamp(this.health + 1, this.health, this.startHealth);
        if (onPlayerAddHealth != null)
        {
            onPlayerAddHealth();
        }
    }

    /// <summary>
    /// Reduce the current health value by damage taken
    /// </summary>
    /// <param name="damage"></param>
    private void TakeDamage(int damage)
    {
        if (_playerScript.Vulnerable)
        {
            health -= damage;
            EmitParticles();
            AudioSource.PlayClipAtPoint(hitClip, transform.position);

            if (health <= 0)
            {
                Die();
            }

            if (onPlayerTakesDamage != null)
            {
                onPlayerTakesDamage();
            }
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
