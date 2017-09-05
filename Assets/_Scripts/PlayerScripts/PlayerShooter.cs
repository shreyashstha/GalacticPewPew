using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class PlayerShooter : MonoBehaviour {

    //*****Public Variables*****
    //Test
    public bool autoShoot = false;
    // Projectile Variables
    public GameObject projectile;       // The projectile to shoot
    public Transform shipNose;          // The transform to shoot the projectile from
    //*****Private Variables*****
    [SerializeField]
    private float attackSpeed = 0.0f;       // Player Attack Speed
    private float attackSpeedCounter = 0.0f;    //Counter for attack speed interval
    private ObjectPool pool;


    //*****Property
    public float AttackSpeed        //Attack Speed property
    {
        get
        {
            return attackSpeed;
        }

        set
        {
            if (value >= (attackSpeed * 0.5) && value <= (attackSpeed * 2)) attackSpeed = value;
        }
    }

    //*****Delegate*****
    //Delegate for those who care about when we shoot
    public delegate void PlayerIsShooting();
    public PlayerIsShooting onPlayerIsShooting;

    // Use this for initialization
    void Start () {
        pool = this.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(projectile, 10);
	}
	
	// Update is called once per frame
	void Update () {
        // Add time to counter
        AdvanceCounter();
        
        if (!GameManager.instance.Paused)
        {
            // Check if player has touched screen then shoot  
            if (Input.touchCount == 1) Shoot();
        }
        if (autoShoot) Shoot();
    }

    private void Shoot()
    {
        if (attackSpeedCounter == 0.0f)
        {
            //Instantiate<GameObject>(projectile, shipNose.position, Quaternion.identity);
            GameObject spawnObject = pool._OBP_GetPooledObject();
            spawnObject.transform.position = shipNose.position;
            spawnObject.transform.rotation = Quaternion.identity;
            spawnObject.SetActive(true);
            DelegatePlayerIsShooting();
            attackSpeedCounter += Time.deltaTime;
        }
    }

    private void AdvanceCounter()
    {
        if (attackSpeedCounter > 0.0f) attackSpeedCounter += Time.deltaTime;
        if (attackSpeedCounter > attackSpeed) attackSpeedCounter = 0.0f;
    }

    /// <summary>
    /// Function that calls the delegate onPlayerTookDamage
    /// </summary>
    public void DelegatePlayerIsShooting()
    {
        if (onPlayerIsShooting != null)
        {
            onPlayerIsShooting();
        }
    }
}
