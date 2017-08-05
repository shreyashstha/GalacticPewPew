using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    public static PowerUpManager instance = null;
    public int dropChance = 35;
    public bool spawnMode = false;
    public float spawnInterval = 0.0f;
    public GameObject[] powerUps;

    private void Awake()
    {
        if (spawnMode)
        {
            InvokeRepeating("SpawnPowerUp", 0.5f, spawnInterval);
        }
    }

    public void DropPowerUp(Vector3 pos)
    {
        Instantiate(powerUps[Random.Range(0, powerUps.Length)], pos, Quaternion.identity);
    }

    private void SpawnPowerUp()
    {
        Instantiate(powerUps[Random.Range(0, powerUps.Length)], this.transform.position, Quaternion.identity);
    }
}
