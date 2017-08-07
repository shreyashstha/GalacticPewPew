using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyFormation_V3 : MonoBehaviour {

    //*****Public variables*****
    //Variables for enemies to spawn
    public GameObject[] enemies;            //List of spawnable enemy gameobjects.
    public Vector3[] spawnPoint;              //Position to spawn enemies.
    public int spawnCount = 0;              //Number of enemies to spawn in a formation. Formation is a set of enemies spawned once (SpawnEnemies coroutine).
    public float initialDelay = 0.0f;        //Time after which this formation is enabled. (Should we change to score?)
    public float formationSpawnInterval = 0.0f;     //Delay between spawning sets of enemies.
    public float formationIntervalReduce = 0.0f;    //Amount of time to reduce delay between spawn times.
    public float formationSpawnMin = 0.0f;          //Minimum spawn interval 
    public float spawnInterval = 0.0f;              //Delay between spawning individual enemies in a formation.
    public float enemyIncrementInterval = 0.0f;      //Time between increasing the type of enemy to spawn.
    private int maxEnemy = 0;               //index for enemy array

    //*****Private variables*****
    private ObjectPool pool;        //Object pool reference


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
        //TODO: Manipulating spawn points
        //spawnPoint = this.transform.position;
        pool = gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(enemies, 10);

        Invoke("StartCoroutines", initialDelay);
    }
    // Update is called once per frame
    void Update () {
		
	}

    void StartCoroutines()
    {
        StartCoroutine(SpawnFormationCoroutine());
        StartCoroutine(SpawnIntervalCoroutine());
        StartCoroutine(MaxEnemyCoroutine());
    }

    IEnumerator SpawnFormationCoroutine()
    {
        while (!GameManager.instance.GameOverBool)
        {
            StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(formationSpawnInterval);
        }

        yield return null;
    }

    IEnumerator SpawnEnemies()
    {
        int index = Random.Range(0, maxEnemy);
        for (int i = 0; i < spawnCount; i++)
        {
            GameObject enemy = pool._OBP_GetPooledObject(index);
            enemy.transform.position = this.transform.position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnIntervalCoroutine()
    {
        while (!GameManager.instance.GameOverBool)
        {
            yield return new WaitForSeconds(60f);
            //formationSpawnInterval -= formationIntervalReduce;
            formationSpawnInterval = Mathf.Clamp(formationSpawnInterval - formationIntervalReduce, formationSpawnMin, formationSpawnInterval);
        }
    }

    IEnumerator MaxEnemyCoroutine()
    {
        while (!GameManager.instance.GameOverBool)
        {
            yield return new WaitForSeconds(enemyIncrementInterval);
            maxEnemy = Mathf.Clamp(maxEnemy + 1, 0, enemies.Length);
            Debug.Log(maxEnemy);
        }
    }
}
