using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class name: EnemyShooter
/// Interfaces: IEnemyShooter, IPoolableObject
/// </summary>
[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour, IPoolableObject {

    //*****Private Variables****
    //Object Pool
    protected ObjectPool pool;            //ObjectPool component reference
    private float asCounter = 0.0f;     //Counter for Attack Speed
    private Enemy thisEnemy;            //Enemy script component
    private int poolIndex = 0;

    //*****Public Variables*****
    public bool shootAnimation = false; //True if shoot animation exists
    //Projectile Variables
    public Transform[] barrel;          //Transforms where projectiles spawn
    public GameObject projectile;       //The projectile GameObject to spawn
    public float attackSpeed = 2.0f;    //How often to attempt to shoot
    public int chanceToShoot = 0;       //Percentage chance to spawn projectile

    private void Awake()
    {
        thisEnemy = gameObject.GetComponent<Enemy>();
    }

    // Use this for initialization
    void Start () {

    }

    //Update checks if ship can attack or not and updates asCounter.
    private void Update()
    {
        asCounter += Time.deltaTime;
        //Shoot if asCounter equals attackSpeed
        if (asCounter >= attackSpeed && !thisEnemy.IsDead)
        {
            Shoot();
            asCounter = 0f;
        }
    }

    public virtual void Shoot()
    {
        int random = Random.Range(0,99);
        if (random < chanceToShoot)
        {
            InstantiateProjectiles();
        }
    }

    protected virtual void InstantiateProjectiles()
    {
        for (int i = 0; i < barrel.Length; i++)
        {
            //Instantiate(projectile, barrel[i].position, Quaternion.identity);
            GameObject spawnObject = pool._OBP_GetPooledObject(poolIndex);
            if (spawnObject != null)
            {
                spawnObject.transform.position = barrel[i].position;
                spawnObject.transform.rotation = barrel[i].rotation;
                spawnObject.SetActive(true);
            }
            else {
                Debug.Log("EnemyShooter null pool");
            }         
        }
    }

    public virtual void OnEnable()
    {
        //asCounter = attackSpeed * 0.5f;
        //Reset attack speed
        asCounter = 0.0f;
        pool = gameObject.GetComponent<ObjectPool>();
        if (pool != null && pool._OBP_PoolLength == 0)
        {
            pool._OBP_ConstructObjectPool(projectile, 3);
        }else if (pool._OBP_PoolLength > 0)
        {
            pool._OBP_AddPooledObject(projectile, 3);
            poolIndex = pool._OBP_PoolLength - 1;
        }
    }

    public void OnDisable()
    {

    }

}
