using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class PowerUpSpawner : MonoBehaviour {

    private ObjectPool pool;
    public int spawnPercent = 5;
    public float spawnInterval = 0f;
    public GameObject PowerUp;

    private void Awake()
    {
        pool = this.gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(PowerUp, 4);
    }

    // Use this for initialization
    void Start () {
        StartCoroutine("SpawnPowerUpCoroutine");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnPowerUpCoroutine()
    {
        while(!GameManager.instance.GameOverBool){
            int spawnChance = Random.Range(0, 99);
            if (spawnChance < spawnPercent)
            {
                SpawnPowerUp(pool._OBP_GetPooledObject());
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPowerUp(GameObject spawnObject)
    {
        Vector3 pos = new Vector3(-8f, (float)Random.Range(-1, 4), 0);
        spawnObject.transform.position = pos;
        spawnObject.transform.rotation = Quaternion.identity;
        spawnObject.SetActive(true);
    }
}
