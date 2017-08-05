using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyFormation_V3 : MonoBehaviour {

    //*****Public variables*****
    //Variables for enemies to spawn
    public GameObject[] enemies;            //List of spawnable enemy gameobjects
    public Vector3 spawnPoint;
    public int spawnCount = 0;
    public float spawnInterval = 0.0f;

    //*****Private variables*****
    private ObjectPool pool;


    // Use this for initialization
    void Start () {
        InitFormation();
	}

    /// <summary>
    /// Initialize Object Pool
    /// Set up the game scene with enemy formation
    /// </summary>
    private void InitFormation()
    {
        //Set up object pooling
        spawnPoint = this.transform.position;
        pool = gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(enemies, 10);

        InvokeRepeating("StartSpawn", 0f, 10f);
    }
    // Update is called once per frame
    void Update () {
		
	}
    void StartSpawn()
    {
        StartCoroutine("SpawnEnemies");
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject enemy = pool._OBP_GetPooledObject();
            enemy.transform.position = this.transform.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
