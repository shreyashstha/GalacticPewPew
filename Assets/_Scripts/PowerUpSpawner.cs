using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class PowerUpSpawner : MonoBehaviour {

    private ObjectPool pool;
    [SerializeField]
    public int spawnPercent = 0;        //Chance to spawn
    [SerializeField]
    public float spawnInterval = 0f;    //Interval between spawns
    [SerializeField]
    public float spawnDelay = 0f;       //Initial delay before spawn
    public GameObject Holder;

    private void Awake()
    {
        pool = this.gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(Holder, 4);
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
        yield return new WaitForSeconds(spawnDelay);

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
        Vector3 pos;
        int random = Random.Range(0, 99);
        if (random < 50)
        {
            pos = new Vector3(-8f, (float)Random.Range(-1, 4), 0);
        }else
        {
            pos = new Vector3(8f, (float)Random.Range(-1, 4), 0);
        }
        spawnObject.transform.position = pos;
        spawnObject.transform.rotation = Quaternion.identity;
        spawnObject.SetActive(true);
    }
}
