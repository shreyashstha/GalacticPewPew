using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyFormation_V3 : MonoBehaviour {

    //*****Public variables*****
    [SerializeField]
    private float initialDelay = 0.0f;                  //Time after which this formation is enabled. (Should we change to score?)
    [SerializeField]
    private float formationSpawnInterval = 0.0f;        //Interval between spawning formations
    [SerializeField]
    private float enemySpawnInterval = 0.0f;              //Delay between spawning individual enemies in a formation.

    //Variables for enemies to spawn
    [SerializeField]
    private GameObject[] enemies;            //List of spawnable enemy gameobjects.
    [SerializeField]
    private int numberOfSpawns = 0;              //Number of enemies to spawn in a formation. Formation is a set of enemies spawned once (SpawnEnemies coroutine).
    [SerializeField]
    private float enemyIncrementInterval = 0.0f;      //Time between increasing the type of enemy to spawn.

    //Variables for Formation
    public Vector3[] spawnPoints;

    //These are handled by coroutines.
    private int maxEnemyIndex = 2;               //index for enemy array.

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

        //Start the game after some delay.
        Invoke("StartCoroutines", initialDelay);
    }
    // Update is called once per frame
    void Update () {

    }

    /// <summary>
    /// Starts Coroutines responsible for the Game Formation.
    /// </summary>
    void StartCoroutines()
    {
        StartCoroutine(SpawnFormationCoroutine());
        StartCoroutine(MaxEnemyCoroutine());
    }
    
    IEnumerator SpawnFormationCoroutine()
    {
        while (!GameManager.instance.GameOverBool)
        {
            //Debug.Log("Spawning Formation.....SpawnFormationDelayedCoroutine....." + Time.time);
            int enemyIndex = Random.Range(0, maxEnemyIndex);
            int spawnCount = Random.Range(4, numberOfSpawns);

            int whichSpawnStyle = Random.Range(0, 99);
            if (whichSpawnStyle < 5)
            {
                Debug.Log("5% Spawn all at once");
                int count = Random.Range(2, 4);
               
                for (int i = 0; i < count; i++)
                {
                    StartCoroutine(SpawnEnemiesCoroutine(enemyIndex, spawnPoints[i], spawnCount));
                }
                yield return new WaitForSeconds(formationSpawnInterval);
            }
            else if (whichSpawnStyle >= 5 && whichSpawnStyle < 25)
            {
                Debug.Log("20% Spawn at smaller intervals");
                int count = 3 + ((int)Time.time / 60);
                for (int i = 0; i < count; i++)
                {
                    int index = Random.Range(0, maxEnemyIndex);
                    StartCoroutine(SpawnEnemiesCoroutine(index, spawnPoints[Random.Range(0, spawnPoints.Length)], spawnCount));
                    yield return new WaitForSeconds(3.5f);
                }
                yield return new WaitForSeconds(formationSpawnInterval);
            }
            else
            {
                Debug.Log("75% Spawn at normal intervals");

                StartCoroutine(SpawnEnemiesCoroutine(enemyIndex, spawnPoints[Random.Range(0, spawnPoints.Length)], spawnCount));
                yield return new WaitForSeconds(formationSpawnInterval);
            }
        }   
    }

    private void SpawnFormation(int points)
    {
        //Debug.Log("Spawning Formation.....SpawnFormation....." + Time.time);

        
    }

    IEnumerator SpawnEnemiesCoroutine(int index, Vector3 position, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = pool._OBP_GetPooledObject(index); 
            enemy.transform.position = position;
            enemy.transform.rotation = Quaternion.identity;
            enemy.SetActive(true);

            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    IEnumerator MaxEnemyCoroutine()
    {
        while (!GameManager.instance.GameOverBool && (maxEnemyIndex < enemies.Length))
        {
            yield return new WaitForSeconds(enemyIncrementInterval);
            maxEnemyIndex = Mathf.Clamp(maxEnemyIndex + 1, 0, enemies.Length);
            //Debug.Log("Incrementing Max Index.....MaxEnemyCoroutine....." + Time.time + " " + maxEnemyIndex);
        }
    }
}
