using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour, IEnemyShooter, IPoolableObject {

    //*****Private Variables****
    //Object Pool
    protected ObjectPool pool;            //ObjectPool component reference
    private float asCounter = 0.0f;     //Counter for Attack Speed
    private Enemy thisEnemy;            //Enemy script component
    //*****Public Variables*****
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
            GameObject spawnObject = pool._OBP_GetPooledObject();
            if (spawnObject != null)
            {
                spawnObject.transform.position = barrel[i].position;
                spawnObject.transform.rotation = Quaternion.identity;
                spawnObject.SetActive(true);
            }
            else {
                Debug.Log("EnemyShooter null pool");
            }         
        }
    }

    public virtual void OnEnable()
    {
        //Assigning a random value to attack speed counter so that all the enemies don't shoot at the same time.
        //asCounter = Random.Range(1.0f, attackSpeed);

        pool = gameObject.GetComponent<ObjectPool>();
        if (pool != null && pool._OBP_PoolLength == 0)
        {
            pool._OBP_ConstructObjectPool(projectile, 3);
        }
    }

    public void OnDisable()
    {

    }

}
