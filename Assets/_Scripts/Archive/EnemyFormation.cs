using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyInformation
{
    public GameObject position;
    public EnemyType type;
    public EnemyPosition positionScript;

    public EnemyInformation(GameObject enemy, EnemyType type, EnemyPosition script)
    {
        this.position = enemy;
        this.type = type;
        this.positionScript = script;
    } 
}

public class EnemyFormation : MonoBehaviour {

    ////Variables for Formation dimentions
    //[SerializeField]
    //private int xDimention = 1;
    //private int yDimention = 1;
    ////Variables for Boundary
    //private float leftmostXPos = 0.0f;
    //private float rightmostXPos = 0.0f;
    //public float boundaryPad = 0.0f;

    //*****Private Constant Variables*****
    private const int CHILDREN_4 = 4;
    private const int CHILDREN_6 = 6;
    private const int CHILDREN_8 = 8;
    private const int CHILDREN = 10;                        //Constant number of child position to spawn

    private const int TUTORIAL_LVL_1 = 50;       //Constant for tut level 1
    private const int TUTORIAL_LVL_2 = 51;       //Constant for tut level 2
    private const int TUTORIAL_LVL_3 = 52;       //Constant for tut level 3
    private int currentNumChildren;

    //*****Private variables*****
    private int changeFormationScore = 0;   //Keeps track of the Score when Formation changed *otherwise keeps changing positions until score updates*
    //Variables for spawning enemies
    [SerializeField]
    private float spawnTimeMin = 0.5f;           //Minimum time to spawn an enemy
    [SerializeField]
    private float spawnTimeMax = 1.25f;           //Maximum time to spawn an enemy
    private float spawnTime = 1.0f;         //Time to wait before next spawn
    private float spawnTimeCounter = 0.0f;  //Counter for next spawn time
    // Variables for Children. Used in Formations
    private ArrayList emptyChildrenList = new ArrayList();  //List of child positions without spawned enemy
    private ArrayList fullChildrenList = new ArrayList();   //List of child positions with spawned enemy
    //private Dictionary<GameObject,EnemyType> children = new Dictionary<GameObject, EnemyType>();
    private EnemyInformation[] children = new EnemyInformation[CHILDREN];

    private int numberOfFormations = 5;     //Total Number of formations
    [SerializeField]
    private int currentFormation = 1;       //Current formation
    private ObjectPool pool;
    
    //*****Public variables*****
    //Variables for enemies to spawn
    public GameObject[] enemies;            //List of spawnable enemy gameobjects
    //TODO:
    public GameObject[] mediumEnemies;      //List of spawnable medium enemy gameobjects
    public bool tutorial = true;           //Is tutorial selected or not


    // FormationCalculator instance
    //private FormationCalculator formationCalculator = new FormationCalculator();


    // Use this for initialization
    void Start ()
	{
        InitFormation();
	}

    // Update is called once per frame
    void Update()
    {
        int currentScore = GameManager.instance.Score;

        //Add another enemy type for spawning every 400 (1/500 = 0.002) points.
        int maxEnemy = (int)(GameManager.instance.Score * 0.002) + 2;

        //if (maxEnemy > 2)
        //{
        //     maxEnemy = enemies.Length - 1;
        //    //Add a pool if Object pool list does not have the ith enemy
        //    if (maxEnemy == pool._OBP_PoolLength)
        //    {
        //        pool._OBP_AddPooledObject(enemies[maxEnemy], 5);
        //    }
        //}
        if (maxEnemy > enemies.Length) maxEnemy = enemies.Length;
        //Spawn enemies if there are positions without child
        SpawnEnemies(maxEnemy);
        //Move empty positions to empty list
        RecycleEmptyChildren();

        //Check score and change formation if needed
        //First three are easy levels.
        if (currentScore >= 200 && currentNumChildren == CHILDREN_4 ){
            AddChildren(2);
            currentNumChildren = CHILDREN_6;
            ChangeFormation(TUTORIAL_LVL_2);
            SpawnAllEnemies();
        }
        else if (currentScore >= 400 && currentNumChildren == CHILDREN_6){
            AddChildren(2);
            currentNumChildren = CHILDREN_8;
            ChangeFormation(TUTORIAL_LVL_3);
            SpawnAllEnemies();
        }
        else if (currentScore >= 600 && currentNumChildren == CHILDREN_8){
            AddChildren(2);
            currentNumChildren = CHILDREN;
            changeFormationScore = 600;
            ChangeFormation(currentFormation);
            SpawnAllEnemies();
            currentFormation++;
        }
        //Changing formations after tutorial stages. Every 750 points.
        else if (currentScore > changeFormationScore){
            if (currentScore % 500 == 0)
            {
                Debug.Log("Changing Formation");
                changeFormationScore = currentScore;
                ChangeFormation(currentFormation);
                SpawnAllEnemies();
                currentFormation++;
                if (currentFormation > numberOfFormations) currentFormation = 1;
                Debug.Log(currentFormation);
            }
        }
    }

    /// <summary>
    /// Initialize Object Pool
    /// Set up the game scene with enemy formation
    /// </summary>
    private void InitFormation()
    {
        //Set up object pooling
        pool = gameObject.GetComponent<ObjectPool>();
        pool._OBP_ConstructObjectPool(enemies, 6);
        //If tutorial mode is on then start with 4 children and tutorial formations
        //Else start with normal formation with 10 children
        if (tutorial)
        {
            currentNumChildren = CHILDREN_4;
            CreateChildren(CHILDREN_4);
            ChangeFormation(TUTORIAL_LVL_1);
        }
        else
        {
            currentNumChildren = CHILDREN;
            CreateChildren(CHILDREN);
            ChangeFormation(currentFormation);
        }
        SpawnAllEnemies();
    }

    /// <summary>
    /// Create gameobjects as children of this formation.
    /// Adds EnemyPosition scrip to child gameobject.
    /// Adds child to emptyChildrenList.
    /// </summary>
    /// <param name="count">int number of children to create</param>
    private void CreateChildren(int count)
    {
        // Create child gameobjects
        for (int i = 0; i < count; i++)
        {
            GameObject child = new GameObject();
            child.name = "Enemy";
            child.transform.parent = this.transform;
            child.transform.position = new Vector3(0f, 6.5f, 0f);
            children[i] = new EnemyInformation(child, EnemyType.shooter, child.AddComponent<EnemyPosition>());
            emptyChildrenList.Add(child);
        }

        //extraChild = new GameObject();
        //extraChild.name = "EnemyParent";
        //extraChild.transform.parent = this.transform;
        //extraChild.transform.position = new Vector3(0f, 6.5f, 0f);
        //extraChildEnemyPostion = extraChild.AddComponent<EnemyPosition>();
    }

    /// <summary>
    /// Create gameobjects as children of this formation after initial CreateChildren
    /// </summary>
    /// <param name="count"></param>
    private void AddChildren(int count)
    {
        for(int i = 0; i < count; i++)
        {
            GameObject child = new GameObject();
            child.name = "Enemy";
            child.transform.parent = this.transform;
            child.transform.position = new Vector3(0f, 6.5f, 0f);
            children[currentNumChildren + i] = new EnemyInformation(child, EnemyType.shooter, child.AddComponent<EnemyPosition>());
            emptyChildrenList.Add(child);
        }
    }

    /// <summary>
    /// Change the current formation
    /// </summary>
    /// <param name="level"></param>
    private void ChangeFormation (int level)
	{
        switch (level)
        {
            case TUTORIAL_LVL_1:
                Formation_beginner_1();
                break;
            case TUTORIAL_LVL_2:
                Formation_beginner_2();
                break;
            case TUTORIAL_LVL_3:
                Formation_beginner_3();
                break;
            case 1:
                Formation_1();
                break;
            case 2:
                Formation_2();
                break;
            case 3:
                Formation_3();
                break;
            case 4:
                Formation_4();
                break;
            case 5:
                Formation_5();
                break;
        }        
    }

    /// <summary>
    /// Iterate over fullChildList and move empty children gameobjects to emptyChildList.
    /// </summary>
    private void RecycleEmptyChildren()
    {
        for (int i = 0; i < fullChildrenList.Count; i++)
        {
            GameObject child = (GameObject)fullChildrenList[i];
            if (child.transform.childCount == 0 )
            {
                fullChildrenList.Remove(child);
                emptyChildrenList.Add(child);
            }
        }
    }

    /// <summary>
    /// Spawns enemy on every child position
    /// </summary>
	private void SpawnAllEnemies ()
	{
		while (emptyChildrenList.Count > 0) {
			GameObject child = (GameObject)emptyChildrenList[0];
			emptyChildrenList.Remove(child);
			fullChildrenList.Add(child);
			SpawnEnemy(child.transform, 1, 2);
	    }
	}

    /// <summary>
    /// Spawns enemies on every child enemy placeholders after spawnTime float.
    /// </summary>
	private void SpawnEnemies (int maxEnemy)
	{
        if (spawnTimeCounter == 0.0f && emptyChildrenList.Count > 0)
        {
            // Get a random emptyChild from emptChildrenList (LinkedList). Move to fullChildrenList.
            GameObject current = (GameObject)emptyChildrenList[Random.Range(0, emptyChildrenList.Count)];
            emptyChildrenList.Remove(current);
            fullChildrenList.Add(current);
            //Spawn an enemy
            if (!current.gameObject.activeInHierarchy)
            {
                current.gameObject.SetActive(true);
            }
            //TODO: add a way to pass type of enemy to spawn
            SpawnEnemy(current.transform, 1, maxEnemy);

            spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
            spawnTimeCounter += Time.deltaTime;

        }else if (spawnTimeCounter > 0.0f && spawnTimeCounter < spawnTime)
        {
            spawnTimeCounter += Time.deltaTime;
        }else if (spawnTimeCounter >= spawnTime)
        {
            spawnTimeCounter = 0.0f;
            spawnTime = Random.Range(spawnTimeMin, spawnTimeMax);
        }
    }

    /// <summary>
    /// Spawns one enemy give a tranfrom
    /// </summary>
    /// <param name="child"></param>
    private void SpawnEnemy(Transform child, int type, int maxEnemy)
    {
        GameObject enemy;

        switch (type){
            case 1:
                //enemy = Instantiate(enemies[Random.Range(Mathf.Clamp(maxEnemy - 3, 0, maxEnemy), maxEnemy)], child.transform.position, Quaternion.identity);
                enemy = pool._OBP_GetPooledObject(Random.Range(Mathf.Clamp(maxEnemy - 3, 0, maxEnemy), maxEnemy));
                enemy.transform.position = child.transform.position;
                enemy.transform.rotation = Quaternion.identity;
                enemy.transform.parent = child.transform;
                enemy.SetActive(true);
                break;
            case 2:
                enemy = Instantiate(mediumEnemies[Random.Range(0, mediumEnemies.Length)], child.transform.position, Quaternion.identity);
                enemy.transform.parent = child.transform;
                break;
        }
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

    private void Formation_1()
    {
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

        //for (int i = 2; i <= 9; i++)
        //{
        //    float x = 0;
        //    float y = 0;
        //    //enemyPositionScripts[i].ChangeMovement(new Vector3(x,y,0f), midpoint, MovementType.upanddown, 3f);
        //}

    }

    /*
     * **1 on top and bottom**
     * **1 circle of 4 in the middle**
     * **2 on each side of the circle**
     */
    private void Formation_3()
    {
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
        children[0].positionScript.ChangeMovement(new Vector3(5.5f,8f,0f), 5.5f,MovementType.upanddown, 5, false);
        children[1].positionScript.ChangeMovement(new Vector3(5.5f, 3f, 0f), 3f, MovementType.leftandright, 5, false);
        children[2].positionScript.ChangeMovement(new Vector3(0.5f, 3f, 0f), 5.5f, MovementType.upanddown, 5, true);
        children[3].positionScript.ChangeMovement(new Vector3(0.5f, 8f, 0f), 3f, MovementType.leftandright, 5, true);

        children[4].positionScript.ChangeMovement(new Vector3(-5.5f, 8f, 0f), 5.5f, MovementType.upanddown, 5, false);
        children[5].positionScript.ChangeMovement(new Vector3(-5.5f, 3f, 0f), -3f, MovementType.leftandright, 5, false);
        children[6].positionScript.ChangeMovement(new Vector3(-0.5f, 3f, 0f), 5.5f, MovementType.upanddown, 5, true);
        children[7].positionScript.ChangeMovement(new Vector3(-0.5f, 8f, 0f), -3f, MovementType.leftandright, 5, true);

        children[8].positionScript.ChangeMovement(new Vector3(4f, 5.5f, 0), new Vector3(3f,5.5f,0f));
        children[9].positionScript.ChangeMovement(new Vector3(-4f, 5.5f, 0), new Vector3(-3f, 5.5f, 0f));
    }

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


    /*************** NOT USED FUNCTIONS ***************/
    /// <summary>
    /// Checks if 
    /// </summary>
    /// <returns></returns>
    private bool AllEnemiesDead()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy && child.childCount != 0) return false;
        }
        return true;
    }

    //void OnDrawGizmos ()
    //{
    //	Gizmos.DrawWireCube(transform.position, new Vector3(xDimention, yDimention, 0));
    //}

    //   /// <summary>
    //   /// Moves the entire Formation left to right and back. Might not be used
    //   /// </summary>
    //private void MoveFormation ()
    //{
    //	//Get the current position, left most x position, and right most x position.
    //	Vector3 newPos = this.transform.position;

    //	//Move Formation
    //	if (moveLeft) {
    //		float newXVal = Mathf.Clamp (newPos.x - formationMoveSpeed * Time.deltaTime, leftmostXPos, rightmostXPos);
    //		newPos.x = newXVal;
    //		this.transform.position = newPos;
    //		if (newPos.x <= leftmostXPos) {
    //			moveLeft = false;
    //		}
    //	} else {
    //		float newX = Mathf.Clamp (newPos.x + formationMoveSpeed * Time.deltaTime, leftmostXPos, rightmostXPos);
    //		newPos.x = newX;
    //		this.transform.position = newPos;
    //		if (newPos.x >= rightmostXPos) {
    //			moveLeft = true;
    //		}
    //	}
    //}

    /// <summary>
    /// Sets the position of children to the given a list of Vector3 positions.
    /// </summary>
    /// <param name="positions">LinkedList of Vector3 Positions</param>
    //private void SetChildrenPosition(LinkedList<Vector3> positions)
    //{
    //    IEnumerator positionEnum = positions.GetEnumerator();
    //    IEnumerator childrenEnum = emptyChildrenList.GetEnumerator();

    //    // If number of children does not equal number of positions in the formation reset children and call SetChildrenPostion
    //    if (positions.Count != this.emptyChildrenList.Count)
    //    {
    //        Debug.Log("Positions and childredn are not the same");
    //        CreateChildren(positions.Count);
    //        SetChildrenPosition(positions);
    //        return;
    //    }

    //    while(positionEnum.MoveNext())
    //    {
    //        if (childrenEnum.MoveNext())
    //        {
    //            GameObject current = (GameObject)childrenEnum.Current;
    //            current.transform.localPosition = (Vector3)positionEnum.Current;
    //            current.SetActive(true);
    //        }
    //    }
    //}

    //private void SetBoundary()
    //   {
    //       leftmostXPos = Boundary.MinimumX() + xDimention * 0.5f + boundaryPad;
    //       rightmostXPos = Boundary.MaximumX() - xDimention * 0.5f - boundaryPad;
    //   }
}
