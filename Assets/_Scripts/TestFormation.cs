using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class TestFormation : MonoBehaviour {

    private ObjectPool pool;
    public GameObject enemy;

    // Variables for Children. Used in Formations
    private ArrayList emptyChildrenList = new ArrayList();  //List of child positions without spawned enemy
    private ArrayList fullChildrenList = new ArrayList();   //List of child positions with spawned enemy
    //private Dictionary<GameObject,EnemyType> children = new Dictionary<GameObject, EnemyType>();
    private EnemyInformation[] children = new EnemyInformation[8];

    // Use this for initialization
    void Start () {
        InitFormation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Initialize Object Pool
    /// Set up the game scene with enemy formation
    /// </summary>
    private void InitFormation()
    {
        //Set up object pooling
        pool = gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(enemy, 6);
        CreateChildren(8);
        Formation_beginner_2();
        SpawnAllEnemies();
        StartCoroutine("SpawnAllEnemiesCoroutine");
    }

    private void CreateChildren(int count)
    {
        // Create child gameobjects
        for (int i = 0; i < count; i++)
        {
            GameObject child = new GameObject();
            child.name = "Enemy";
            child.transform.parent = this.transform;
            child.transform.position = new Vector3(10f, 0f, 0f);
            children[i] = new EnemyInformation(child, EnemyType.shooter, child.AddComponent<EnemyPosition>());
            emptyChildrenList.Add(child);
        }
    }

    IEnumerator SpawnAllEnemiesCoroutine()
    {
        SpawnAllEnemies();
        yield return new WaitForSeconds(10f);
    }

    private void SpawnAllEnemies()
    {
        while (emptyChildrenList.Count > 0)
        {
            GameObject child = (GameObject)emptyChildrenList[0];
            emptyChildrenList.Remove(child);
            fullChildrenList.Add(child);
            SpawnEnemy(child.transform);
        }
    }

    private void SpawnEnemy(Transform child)
    {
        enemy = pool._OBP_GetPooledObject();
        enemy.transform.position = child.transform.position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.parent = child.transform;
        enemy.SetActive(true);
    }

    private void Formation_beginner_1()
    {
        children[0].positionScript.ChangeMovement(new Vector3(5.5f, 8f, 0f), 0f, MovementType.leftandright, 11f, false);
        children[2].positionScript.ChangeMovement(new Vector3(2.5f, 6.5f, 0f), 0f, MovementType.leftandright, 11f, false);
        children[3].positionScript.ChangeMovement(new Vector3(-2.5f, 5f, 0f), 0f, MovementType.leftandright, 11f);
        children[1].positionScript.ChangeMovement(new Vector3(-5.5f, 3.5f, 0f), 0f, MovementType.leftandright, 11f);
    }

    private void Formation_beginner_2()
    {
        Vector3 p0 = new Vector3(0, 5, 0);
        Vector3 p1 = new Vector3(-7, 0, 0);
        Vector3 p2 = new Vector3(-7, 10, 0);
        Vector3 p3 = new Vector3(0, 5, 0);

        Vector3 p4 = new Vector3(0, 5, 0);
        Vector3 p5 = new Vector3(7, 0, 0);
        Vector3 p6 = new Vector3(7, 10, 0);
        Vector3 p7 = new Vector3(0, 5, 0);

        Vector3[] points1 = BezierPostions(p0, p1, p2, p3, 200);
        Vector3[] points2 = BezierPostions(p4, p5, p6, p7, 200);
        Vector3[] points = new Vector3[points1.Length + points2.Length];
        Array.Copy(points1, points, points1.Length);
        Array.Copy(points2, 0, points, points1.Length, points2.Length);
        children[0].positionScript.ChangeMovement(points, 0 * (200 / 8));
        children[1].positionScript.ChangeMovement(points, 1 * (200 / 8));
        children[2].positionScript.ChangeMovement(points, 2 * (200 / 8));
        children[3].positionScript.ChangeMovement(points, 3 * (200 / 8));
        children[4].positionScript.ChangeMovement(points, 4 * (200 / 8));
        children[5].positionScript.ChangeMovement(points, 5 * (200 / 8));
        children[6].positionScript.ChangeMovement(points, 6 * (200 / 8));
        children[7].positionScript.ChangeMovement(points, 7 * (200 / 8));
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
            Vector3 point = BezierPoint(p0, p1, p2, p3, ((float)i / (float)steps));
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
