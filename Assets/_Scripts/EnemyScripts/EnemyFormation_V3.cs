using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class EnemyFormation_V3 : MonoBehaviour {

    //*****Private variables*****
    [SerializeField]
    private float initialDelay = 0.0f;                  //Time after which this formation is enabled. (Should we change to score?)
    [SerializeField]
    private float formationSpawnInterval = 0.0f;        //Interval between spawning formations
    [SerializeField]
    private float enemySpawnInterval = 0.0f;              //Delay between spawning individual enemies in a formation.
    [SerializeField]
    private float enemyIncrementInterval = 0.0f;        //Time between increasing the type of enemy to spawn.
    [SerializeField]
    private GameObject[] enemies;            //List of spawnable enemy gameobjects.
    [SerializeField]
    private int numberOfSpawns = 0;              //Number of enemies to spawn in a formation. Formation is a set of enemies spawned once (SpawnEnemies coroutine).
    [SerializeField]
    private int numFormationBeforeHorde = 0;    //Number of formations to spawn before spawning a horde
    [SerializeField]
    private int maxHorde = 6;       //Maximum number of formations in a horde
    private ObjectPool pool;        //Object pool reference

    //These are handled by coroutines.
    private int maxEnemyIndex = 2;          //index for enemy array.

    //*****Public variables*****
    //Variables for Formation
    //public Vector3[] spawnPoints;
    public Vector3[] leftPoints;
    public Vector3[] rightPoints;
    private bool spawnOnRight = false;

    // Use this for initialization
    void Start () {
        maxEnemyIndex = Mathf.Clamp(maxEnemyIndex, 1, enemies.Length);
        InitFormation();
	}

    /// <summary>
    /// Initialize Object Pool
    /// Set up the game scene with enemy formation
    /// *Calls StartCoroutines
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
        StartCoroutine(MaxEnemyCoroutine(enemyIncrementInterval));
        StartCoroutine(IncreaseDifficultyCoroutine());
    }
    
    /// <summary>
    /// Coroutine that is responsible for deciding when to spawn enemy formations and enemy hordes.
    /// Formations - a group of the same enemy at one spawn point
    /// Hordes - a gourp of Formations
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnFormationCoroutine()
    {
        int hordeCountDown = numFormationBeforeHorde;
        while (!GameManager.instance.GameOverBool)
        {
            //Choose random enemy form 0 to maxEnemyIndex
            int enemyIndex = Random.Range(0, maxEnemyIndex);
            //Choose number of enemies to spawn
            int spawnCount = Random.Range(4, numberOfSpawns + 1);

            //Choose a spawnPoint
            StartCoroutine(SpawnEnemiesCoroutine(enemyIndex, GetSpawnPoint(), spawnCount));
            yield return new WaitForSeconds(formationSpawnInterval);

            ////Check if we need to spawn a horde
            //if (hordeCountDown == 0)
            //{
            //    //Number of formation
            //    int count = 3 + ((int)Time.time / 90);
            //    count = Mathf.Clamp(count, 3, maxHorde);
            //    for (int i = 0; i < count; i++)
            //    {
            //        //Choose random enemy
            //        int index = Random.Range(0, maxEnemyIndex);
            //        StartCoroutine(SpawnEnemiesCoroutine(index, GetSpawnPoint(), 5));
            //        yield return new WaitForSeconds(4.25f);
            //    }
            //    hordeCountDown = numFormationBeforeHorde;
            //    yield return new WaitForSeconds(formationSpawnInterval);
            //}
            //else
            //{
            //    //Choose a spawnPoint
            //    StartCoroutine(SpawnEnemiesCoroutine(enemyIndex, GetSpawnPoint(), spawnCount));
            //    hordeCountDown--;
            //    yield return new WaitForSeconds(formationSpawnInterval);
            //}
        }   
    }

    //Just gets a random spawn point on left or right (bool spawnOnRight)
    Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint;
        if (spawnOnRight)
        {
            spawnPoint = rightPoints[Random.Range(0, rightPoints.Length)];
            spawnOnRight = !spawnOnRight;
        }
        else
        {
            spawnPoint = leftPoints[Random.Range(0, leftPoints.Length)];
            spawnOnRight = !spawnOnRight;
        }
        return spawnPoint;
    }

    /// <summary>
    /// Coroutine that will spawn a formation.
    /// </summary>
    /// <param name="index">index of the enemy to spawn in the object poop (also enemies array)</param>
    /// <param name="position">Vector3 position of the new gameobject</param>
    /// <param name="count">number of enemies to spawn</param>
    /// <returns></returns>
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

    /// <summary>
    /// Coroutine that increments Max Enemy index that can be spawned.
    /// </summary>
    /// <returns></returns>
    IEnumerator MaxEnemyCoroutine(float incrementInterval)
    {
        while (!GameManager.instance.GameOverBool && (maxEnemyIndex < enemies.Length))
        {
            yield return new WaitForSeconds(incrementInterval);
            maxEnemyIndex = Mathf.Clamp(maxEnemyIndex + 1, 0, enemies.Length);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator IncreaseDifficultyCoroutine()
    {
        while (!GameManager.instance.GameOverBool)
        {
            yield return new WaitForSeconds(90f);
            formationSpawnInterval = Mathf.Clamp(formationSpawnInterval - 1f, 6f, formationSpawnInterval);
        }
    }
}
