using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class TestScript : MonoBehaviour {

    private Transform player;
    public float speed = 0.0f;
    private Transform childGO;

    public GameObject enemy;
    public float spawnInterval = 0.0f;
    private ObjectPool pool;

    private void Awake()
    {
        pool = gameObject.GetComponent<ObjectPool>();
    }

    // Use this for initialization
    void Start () {
        PrintTest();
        //pool._OBP_ConstructObjectPool(enemy, 5);
        //StartCoroutine(SpawnEnemy(enemy,spawnInterval));
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    private void Enable()
    {
        childGO.gameObject.SetActive(true);
    }

    private void Disable()
    {
        childGO.gameObject.SetActive(false);
    }

    private void PrintTest()
    {
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(-5, -3, 0);
        Vector3 p2 = new Vector3(-5, 3, 0);
        Vector3 p3 = new Vector3(0, 0, 0);

        Vector3[] points = BezierPostions(p0, p1, p2, p3, 5);
    }

    private int RandomOddNumber()
    {
        int num = Random.Range(1, 7);
        if (num % 2 == 0) return num + 1;
        else return num;
    }

    private void MoveTowardsPlayer()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);
    }

    private void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Do Something!");
		}
	}

    IEnumerator SpawnEnemy(GameObject enemy, float interval)
    {
        GameObject spawnEnemy = pool._OBP_GetPooledObject();
        spawnEnemy.transform.position = this.transform.position;
        spawnEnemy.transform.rotation = this.transform.rotation;
        spawnEnemy.SetActive(true);

        yield return new WaitForSeconds(interval);
    }

    /// <summary>
    /// Given 4 Vector3, two control points, a start point and end point
    /// </summary>
    /// <param name="p0">Start Point</param>
    /// <param name="p1">First Control Point</param>
    /// <param name="p2">Second Control Point</param>
    /// <param name="p3">End Point</param>
    /// <param name="steps">Number of linear interpolations - Makes curve smoother</param>
    /// <returns></returns>
    Vector3[] BezierPostions(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int steps)
    {
        Vector3[] points = new Vector3[steps];

        for (int i = 0; i < steps; i++)
        {
            Vector3 point = BezierPoint(p0, p1, p2, p3, ((float)i /(float)steps));
            points[i] = point;
        }

        return points;
    }

    /// <summary>
    /// Interpolates between Bezier curve at t
    /// </summary>
    /// <param name="p0">Start Point</param>
    /// <param name="p1">First Control Point</param>
    /// <param name="p2">Second Control Point</param>
    /// <param name="p3">End Point</param>
    /// <param name="t">t value [0,1] percent interpolation</param>
    /// <returns></returns>
    Vector3 BezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float mt3 = (1 - t) * (1 - t) * (1 - t);
        float mt2 = (1 - t) * (1 - t);
        float t2 = t * t;
        float t3 = t * t * t;

        return (p0 * mt3) + (3 * p1 * mt2 * t) + (3 * p2 * (1 - t) * t2) + (p3 * t3);
    }
}
