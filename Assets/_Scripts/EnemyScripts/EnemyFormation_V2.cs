using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    shooter,
    knuckle,
    muscle,
    rocket,
    galecto,
    dragonfly,
    poodle,
    saucer,
    ufo,
    bat,
    martian,
    binocular
}
public enum EnemyPositionState
{
    unused,
    empty,
    full
}

public class EnemyInformation_V2
{
    public GameObject position;
    private EnemyPositionState state;
    public EnemyPositionState State
    {
        get
        {
            return state;
        }

        set
        {
            state = value;
        }
    }
    public EnemyPosition positionScript;

    public EnemyInformation_V2(GameObject enemy, EnemyPositionState state, EnemyPosition script)
    {
        this.position = enemy;
        this.state = state;
        this.positionScript = script;
    }
}

[RequireComponent(typeof(ObjectPool))]
public class EnemyFormation_V2 : MonoBehaviour {

    //*****Private Constant variables*****
    private const int CHILDREN = 10;
    private const int TUTORIAL_LVL_1 = 50;       //Constant for tut level 1
    private const int TUTORIAL_LVL_2 = 51;       //Constant for tut level 2
    private const int TUTORIAL_LVL_3 = 52;       //Constant for tut level 3
    private const int TUTORIAL_LVL_4 = 53;       //Constant for tut level 3

    //*****Private variables*****
    private int changeFormationScore = 0;   //Keeps track of the Score when Formation changed *otherwise keeps changing positions until score updates*
    private int spawnCountTotal = 0;
    private int spawnCountCurrent = 0;
    
    //Variables for spawning enemies
    [SerializeField]
    private float spawnTimeMin = 0.5f;           //Minimum time to spawn an enemy
    [SerializeField]
    private float spawnTimeMax = 1.25f;           //Maximum time to spawn an enemy
    private float spawnTime = 1.0f;         //Time to wait before next spawn
    private float spawnTimeCounter = 0.0f;  //Counter for next spawn time
    private GameObject[] enemiesToSpawnForCurrentFormation;

    //Variables for Children. Used in Formations
    private EnemyInformation_V2[] children = new EnemyInformation_V2[CHILDREN];

    //Variables for formation numbers
    private int numberOfFormations = 9;     //Total Number of formations
    [SerializeField]
    private int currentFormation = 1;       //Current formation
    private ObjectPool pool;

    //*****Public variables*****
    //Variables for enemies to spawn
    public GameObject[] enemies;            //List of spawnable enemy gameobjects
    //TODO:
    public GameObject[] mediumEnemies;      //List of spawnable medium enemy gameobjects
    public bool tutorial = true;           //Is tutorial selected or not

    // *Start here*
    private void Start () {
        InitFormation();
    }

    /// <summary>
    /// Initialize Object Pool
    /// Set up the game scene with enemy formation
    /// </summary>
    private void InitFormation()
    {
        //Set up object pooling
        pool = gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(enemies, 10);

        //If tutorial mode
        //Else start with normal formation
        if (tutorial)
        {
            CreateChildren();
            ChangeFormation(1);
            changeFormationScore += 100;
        }
        else
        {
            CreateChildren();
            ChangeFormation(5);
            changeFormationScore += 1000;
        }
        SpawnAllEnemies();
    }

    /// <summary>
    /// Create gameobjects as children of this formation.
    /// Adds EnemyPosition scrip to child gameobject.
    /// Adds child to emptyChildrenList.
    /// Should be called only once.
    /// </summary>
    /// <param name="count">int number of children to create</param>
    private void CreateChildren()
    {
        // Create child gameobjects
        for (int i = 0; i < CHILDREN; i++)
        {
            GameObject child = new GameObject();
            child.name = "Enemy";
            child.transform.parent = this.transform;
            child.transform.position = new Vector3(0f, 6.5f, 0f);
            children[i] = new EnemyInformation_V2(child, EnemyPositionState.unused, child.AddComponent<EnemyPosition>());
        }
    }

    /// <summary>
    /// Sets the EnemyPositionState to empty for count children. These children will be used to spawn enemies
    /// </summary>
    /// <param name="count"></param>
    private void ActivateChildren(int count)
    {
        for (int i = 0; i < children.Length - (children.Length - count); i++)
        {
            children[i].State = EnemyPositionState.empty;
        }
    }

    private void DeactivateChildren()
    {
        for (int i = 0; i < children.Length; i++)
        {
            children[i].State = EnemyPositionState.unused;
        }
    }

    /// <summary>
    /// Iterate over children and change the state of children from full to empty if no child exists.
    /// </summary>
    private void RecycleChildren()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].State == EnemyPositionState.full)
            {
                if (children[i].position.transform.childCount == 0)
                {
                    children[i].State = EnemyPositionState.empty;
                }
            }
        }
    }

    /// <summary>
    /// Checks if all enemies are dead. Children have not children
    /// </summary>
    /// <returns></returns>
    private bool AllEnemiesDead()
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].State == EnemyPositionState.full && children[i].position.transform.childCount != 0) return false;
        }
        return true;
    }

    /// <summary>
    /// Change the current formation
    /// </summary>
    /// <param name="level"></param>
    private void ChangeFormation(int level)
    {
        spawnCountCurrent = 0;
        switch (level)
        {
            case 1:
                Formation_beginner_1();
                break;
            case 2:
                Formation_beginner_2();
                break;
            case 3:
                Formation_beginner_3();
                break;
            case 4:
                Formation_beginner_4();
                break;
            case 5:
                Formation_1();
                break;
            case 6:
                Formation_2();
                break;
            case 7:
                Formation_3();
                break;
            case 8:
                Formation_4();
                break;
            case 9:
                Formation_5();
                break;
        }
        SpawnAllEnemies();
    }

    /// <summary>
    /// Spawns enemy on every child position
    /// </summary>
	private void SpawnAllEnemies()
    {
        foreach (EnemyInformation_V2 enemy in children)
        {
            if (enemy.State == EnemyPositionState.empty)
            {
                GameObject child = enemy.position;
                enemy.State = EnemyPositionState.full;
                SpawnEnemy(child.transform);
                spawnCountCurrent++;
            }
        }
    }

    /// <summary>
    /// Spawns enemies on every child enemy placeholders after spawnTime float.
    /// </summary>
	private void SpawnEnemies()
    {
        //If timer is 0 spawn enemy
        if (spawnTimeCounter == 0.0f && spawnCountCurrent < spawnCountTotal)
        {
            //Get a child with state empty from children ArrayList
            foreach (EnemyInformation_V2 enemy in children)
            {
                if (enemy.State == EnemyPositionState.empty)
                {
                    GameObject child = enemy.position;
                    enemy.State = EnemyPositionState.full;
                    SpawnEnemy(child.transform);
                    spawnCountCurrent++;
                    //Break so that not all enemies are spawned only one.
                    break;
                }
            }
            //Get new random spawn time
            spawnTime = UnityEngine.Random.Range(spawnTimeMin, spawnTimeMax);
            spawnTimeCounter += Time.deltaTime;
        }
        else if (spawnTimeCounter > 0.0f && spawnTimeCounter < spawnTime)
        {
            spawnTimeCounter += Time.deltaTime;
        }
        else if (spawnTimeCounter >= spawnTime)
        {
            spawnTimeCounter = 0.0f;
            spawnTime = UnityEngine.Random.Range(spawnTimeMin, spawnTimeMax);
        }
    }

    /// <summary>
    /// Spawns one enemy give a tranfrom
    /// </summary>
    /// <param name="child"></param>
    private void SpawnEnemy(Transform child)
    {
        GameObject enemy;
        if (enemiesToSpawnForCurrentFormation.Length > 1)
        {
            enemy = pool._OBP_GetPooledObject(UnityEngine.Random.Range(0, enemiesToSpawnForCurrentFormation.Length));
        }
        else
        {
            enemy = pool._OBP_GetPooledObject(0);
        }

        if (enemy == null)
        {
            Debug.Log("Null object: SpawnEnemy EnemyFormation");
        }

        enemy.transform.position = child.transform.position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.parent = child.transform;
        enemy.SetActive(true);
    }

    // Update is called once per frame
    void Update ()
    {
        //int currentScore = GameManager.instance.Score;
        ////Check score and change formation if needed
        ////First three are easy levels.
        //if (tutorial)
        //{
        //    if (currentScore > 100)
        //    {
        //        ChangeFormation(TUTORIAL_LVL_2);
        //        SpawnAllEnemies();
        //    }
        //    else if (currentScore > 300)
        //    {
        //        ChangeFormation(TUTORIAL_LVL_3);
        //        SpawnAllEnemies();
        //    }
        //    else if (currentScore > 600)
        //    {
        //        ChangeFormation(TUTORIAL_LVL_4);
        //        SpawnAllEnemies();
        //        changeFormationScore = 1000;
        //        //Tutorial is over
        //        tutorial = false;
        //    }
        //}
        ////Changing formations after tutorial stages. Every 750 points.
        //else if(currentScore > changeFormationScore)
        //{
        //    changeFormationScore += 500;
        //    ChangeFormation(currentFormation);
        //    SpawnAllEnemies();
        //    currentFormation++;
        //    if (currentFormation > numberOfFormations) currentFormation = 1;
        //}

        if ((spawnCountCurrent == spawnCountTotal) && AllEnemiesDead())
        {
            DeactivateChildren();
            currentFormation++;
            if (currentFormation > numberOfFormations) currentFormation = 5;
            ChangeFormation(currentFormation);
        }
        //Spawn enemies if there are positions without child
        SpawnEnemies();
        //Move empty positions to empty list
        RecycleChildren();
    }

    //******************** FORMATIONS ********************
    private void Formation_beginner_1()
    {
        spawnCountTotal = 10;
        ActivateChildren(4);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.shooter] };
        children[0].positionScript.ChangeMovement(new Vector3(5.5f, 8f, 0f), 0f, MovementType.leftandright, 11f, false);
        children[2].positionScript.ChangeMovement(new Vector3(2.5f, 6.5f, 0f), 0f, MovementType.leftandright, 11f, false);
        children[3].positionScript.ChangeMovement(new Vector3(-2.5f, 5f, 0f), 0f, MovementType.leftandright, 11f);
        children[1].positionScript.ChangeMovement(new Vector3(-5.5f, 3.5f, 0f), 0f, MovementType.leftandright, 11f);
    }

    private void Formation_beginner_2()
    {
        spawnCountTotal = 20;
        ActivateChildren(6);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.shooter], enemies[(int)EnemyType.knuckle] };

        children[0].positionScript.ChangeMovement(new Vector3(-5.5f, 8f, 0f), 0f, MovementType.leftandright, 11f);
        children[1].positionScript.ChangeMovement(new Vector3(5.5f, 3f, 0f), 0f, MovementType.leftandright, 11f, false);

        float radius = 1.75f;
        Vector3 center = new Vector3(0f, 5.5f, 0f);
        Vector3[] positionCircle = PositionsInCircle(radius, 2);

        children[2].positionScript.ChangeMovement((center + positionCircle[0]), center);
        children[3].positionScript.ChangeMovement((center + positionCircle[1]), center);

        children[4].positionScript.ChangeMovement(new Vector3(-4f, 5.5f, 0f), MovementType.upanddown, 2.5f, false);
        children[5].positionScript.ChangeMovement(new Vector3(4f, 5.5f, 0f), MovementType.upanddown, 2.5f);
    }

    private void Formation_beginner_3()
    {
        spawnCountTotal = 30;
        ActivateChildren(8);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.muscle], enemies[(int)EnemyType.knuckle] };

        float radius = 2.5f;
        Vector3 centerLeft = new Vector3(3f, 5f, 0f);
        Vector3 centerRight = new Vector3(-3f, 5f, 0f);
        Vector3[] positionLeftCircle = PositionsInCircle(radius, 4);
        Vector3[] positionRightCircle = PositionsInCircle(radius, 4);

        children[0].positionScript.ChangeMovement((centerLeft + positionLeftCircle[0]), centerLeft);
        children[1].positionScript.ChangeMovement((centerLeft + positionLeftCircle[1]), centerLeft);
        children[2].positionScript.ChangeMovement((centerLeft + positionLeftCircle[2]), centerLeft);
        children[3].positionScript.ChangeMovement((centerLeft + positionLeftCircle[3]), centerLeft);

        children[4].positionScript.ChangeMovement((centerRight + positionRightCircle[0]), centerRight);
        children[5].positionScript.ChangeMovement((centerRight + positionRightCircle[1]), centerRight);
        children[6].positionScript.ChangeMovement((centerRight + positionRightCircle[2]), centerRight);
        children[7].positionScript.ChangeMovement((centerRight + positionRightCircle[3]), centerRight);
    }

    private void Formation_beginner_4()
    {
        spawnCountTotal = 30;
        ActivateChildren(8);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.shooter], enemies[(int)EnemyType.knuckle], enemies[(int)EnemyType.muscle] };

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

    private void Formation_1()
    {
        spawnCountTotal = 50;
        ActivateChildren(10);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.shooter], enemies[(int)EnemyType.knuckle], enemies[(int)EnemyType.poodle] };

        children[0].positionScript.ChangeMovement(new Vector3(-4f, 8f, 0f), MovementType.leftandright, 3f);
        children[1].positionScript.ChangeMovement(new Vector3(0f, 8f, 0f), MovementType.leftandright, 3f);
        children[2].positionScript.ChangeMovement(new Vector3(4f, 8f, 0f), MovementType.leftandright, 3f, false);

        children[3].positionScript.ChangeMovement(new Vector3(-4.5f, 6f, 0f), MovementType.leftandright, 2f, false);
        children[4].positionScript.ChangeMovement(new Vector3(-1.5f, 6f, 0f), MovementType.leftandright, 2f, false);
        children[5].positionScript.ChangeMovement(new Vector3(1.5f, 6f, 0f), MovementType.leftandright, 2f);
        children[6].positionScript.ChangeMovement(new Vector3(4.5f, 6f, 0f), MovementType.leftandright, 2f);

        children[7].positionScript.ChangeMovement(new Vector3(-4f, 1f, 0f), MovementType.leftandright, 3f);
        children[8].positionScript.ChangeMovement(new Vector3(0f, 1f, 0f), MovementType.leftandright, 3f, false);
        children[9].positionScript.ChangeMovement(new Vector3(4f, 1f, 0f), MovementType.leftandright, 3f, false);
    }

    /*
    ****1 ship left right
    ****8 ships up and down
    ****1 ship left and right
    */
    private void Formation_2()
    {
        spawnCountTotal = 50;
        ActivateChildren(10);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.poodle], enemies[(int)EnemyType.knuckle], enemies[(int)EnemyType.rocket] };

        children[0].positionScript.ChangeMovement(new Vector3(1f, 8f, 0f), MovementType.leftandright, 9f);
        children[1].positionScript.ChangeMovement(new Vector3(1f, 3f, 0f), MovementType.leftandright, 9f);
        children[2].positionScript.ChangeMovement(new Vector3(-1f, 8f, 0f), MovementType.leftandright, 9f);
        children[3].positionScript.ChangeMovement(new Vector3(-1f, 3f, 0f), MovementType.leftandright, 9f);

        float midpoint = 5.5f;
        float startpoint = 5f;
        float interval = 2f;

        children[4].positionScript.ChangeMovement(new Vector3(startpoint - interval * 0, 4f, 0f), midpoint, MovementType.upanddown, 3f);
        children[5].positionScript.ChangeMovement(new Vector3(startpoint - interval * 1, 5f, 0f), midpoint, MovementType.upanddown, 3f);
        children[6].positionScript.ChangeMovement(new Vector3(startpoint - interval * 2, 6f, 0f), midpoint, MovementType.upanddown, 3f);
        children[7].positionScript.ChangeMovement(new Vector3(startpoint - interval * 3, 4f, 0f), midpoint, MovementType.upanddown, 3f);
        children[8].positionScript.ChangeMovement(new Vector3(startpoint - interval * 4, 5f, 0f), midpoint, MovementType.upanddown, 3f);
        children[9].positionScript.ChangeMovement(new Vector3(startpoint - interval * 5, 6f, 0f), midpoint, MovementType.upanddown, 3f);
    }

    /*
     * **1 on top and bottom**
     * **1 circle of 4 in the middle**
     * **2 on each side of the circle**
     */
    private void Formation_3()
    {
        spawnCountTotal = 50;
        ActivateChildren(10);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.poodle], enemies[(int)EnemyType.galecto], enemies[(int)EnemyType.dragonfly] };

        children[0].positionScript.ChangeMovement(new Vector3(0f, 8f, 0f), MovementType.leftandright, 11f);
        children[1].positionScript.ChangeMovement(new Vector3(-4.5f, 6.5f, 0f), MovementType.leftandright, 2f);
        children[2].positionScript.ChangeMovement(new Vector3(-4.5f, 4.5f, 0f), MovementType.leftandright, 2f, false);
        children[3].positionScript.ChangeMovement(new Vector3(4.5f, 6.5f, 0f), MovementType.leftandright, 2f);
        children[4].positionScript.ChangeMovement(new Vector3(4.5f, 4.5f, 0f), MovementType.leftandright, 2f, false);
        children[5].positionScript.ChangeMovement(new Vector3(0f, 3f, 0f), MovementType.leftandright, 11f, false);

        float radius = 1.5f;
        Vector3 center = new Vector3(0f, 5.5f, 0f);
        Vector3[] positionCircle = PositionsInCircle(radius, 4);

        children[6].positionScript.ChangeMovement((center + positionCircle[0]), center);
        children[7].positionScript.ChangeMovement((center + positionCircle[1]), center);
        children[8].positionScript.ChangeMovement((center + positionCircle[2]), center);
        children[9].positionScript.ChangeMovement((center + positionCircle[3]), center);
    }

    /*
     * **2 circles of 5**
     */
    private void Formation_4()
    {
        spawnCountTotal = 50;
        ActivateChildren(10);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.shooter], enemies[(int)EnemyType.galecto], enemies[(int)EnemyType.dragonfly] };

        float radius = 2.5f;
        Vector3 centerLeft = new Vector3(3f, 5f, 0f);
        Vector3 centerRight = new Vector3(-3f, 5f, 0f);
        Vector3[] positionLeftCircle = PositionsInCircle(radius, 5);
        Vector3[] positionRightCircle = PositionsInCircle(radius, 5);

        children[0].positionScript.ChangeMovement((centerLeft + positionLeftCircle[0]), centerLeft);
        children[1].positionScript.ChangeMovement((centerLeft + positionLeftCircle[1]), centerLeft);
        children[2].positionScript.ChangeMovement((centerLeft + positionLeftCircle[2]), centerLeft);
        children[3].positionScript.ChangeMovement((centerLeft + positionLeftCircle[3]), centerLeft);
        children[4].positionScript.ChangeMovement((centerLeft + positionLeftCircle[4]), centerLeft);

        children[5].positionScript.ChangeMovement((centerRight + positionRightCircle[0]), centerRight);
        children[6].positionScript.ChangeMovement((centerRight + positionRightCircle[1]), centerRight);
        children[7].positionScript.ChangeMovement((centerRight + positionRightCircle[2]), centerRight);
        children[8].positionScript.ChangeMovement((centerRight + positionRightCircle[3]), centerRight);
        children[9].positionScript.ChangeMovement((centerRight + positionRightCircle[4]), centerRight);
    }

    private void Formation_5()
    {
        spawnCountTotal = 50;
        ActivateChildren(10);
        enemiesToSpawnForCurrentFormation = new GameObject[] { enemies[(int)EnemyType.saucer], enemies[(int)EnemyType.galecto], enemies[(int)EnemyType.poodle] };

        children[0].positionScript.ChangeMovement(new Vector3(5.5f, 8f, 0f), 5.5f, MovementType.upanddown, 5, false);
        children[1].positionScript.ChangeMovement(new Vector3(5.5f, 3f, 0f), 3f, MovementType.leftandright, 5, false);
        children[2].positionScript.ChangeMovement(new Vector3(0.5f, 3f, 0f), 5.5f, MovementType.upanddown, 5, true);
        children[3].positionScript.ChangeMovement(new Vector3(0.5f, 8f, 0f), 3f, MovementType.leftandright, 5, true);

        children[4].positionScript.ChangeMovement(new Vector3(-5.5f, 8f, 0f), 5.5f, MovementType.upanddown, 5, false);
        children[5].positionScript.ChangeMovement(new Vector3(-5.5f, 3f, 0f), -3f, MovementType.leftandright, 5, false);
        children[6].positionScript.ChangeMovement(new Vector3(-0.5f, 3f, 0f), 5.5f, MovementType.upanddown, 5, true);
        children[7].positionScript.ChangeMovement(new Vector3(-0.5f, 8f, 0f), -3f, MovementType.leftandright, 5, true);

        children[8].positionScript.ChangeMovement(new Vector3(4f, 5.5f, 0), new Vector3(3f, 5.5f, 0f));
        children[9].positionScript.ChangeMovement(new Vector3(-4f, 5.5f, 0), new Vector3(-3f, 5.5f, 0f));
    }
    //******************** END FORMATIONS ********************

    //******************** Formation Helper ********************

    /// <summary>
    /// Finds count number of equidistanct positions in a circle at radius
    /// </summary>
    /// <param name="radius">Radius of the circle</param>
    /// <param name="count">Number of equidistant positions</param>
    /// <returns></returns>
    private Vector3[] PositionsInCircle(float radius, int count)
    {
        if (count <= 0) count = 1;
        Vector3[] positionArray = new Vector3[count];
        float angle = 360f / count;
        for (int i = 0; i < count; i++)
        {
            positionArray[i] = Quaternion.AngleAxis(angle * i, Vector3.forward) * new Vector3(0, radius, 0);
        }

        return positionArray;
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

    //******************** Formation Helper ********************
}
