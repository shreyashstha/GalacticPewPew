using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerScript))]
public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    private int startHealth = 100;          //Player Starting Health
    private int health = 0;                 //Current health
    private PlayerScript _playerScript;     //PlayerScript Component

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

    //Delegate for taking damage
    public delegate void PlayerTakesDamage();
    public PlayerTakesDamage onPlayerTakesDamage;

    private void Awake()
    {
        _playerScript = gameObject.GetComponent<PlayerScript>();
    }

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
        if (_playerScript.Vulnerable)
        {
            health -= damage;
            EmitParticles();

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
        GameManager.instance.SetGameOver();
        gameObject.SetActive(true);
        Destroy(this.gameObject, 2.5f);
    }
}
