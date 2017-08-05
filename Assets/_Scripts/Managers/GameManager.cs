using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //*****Singleton*****
    public static GameManager instance = null;                      //Singleton instance

    //Player
    [SerializeField]
    private GameObject player;                                      //Player object in the scene
    private GameObject activePlayer;                                //Current player on the scene
    public GameObject Player
    {
        get
        {
            return activePlayer;
        }
    }

    private PlayerHealth playerHealth;                              //PlayerHealth component of the Player gameobject

    private PlayerShooter playerShooter;                             //PlayerShooter component of the Player gameobject
    public PlayerShooter PlayerShooter
    {
        get
        {
            return playerShooter;
        }
    }

    [SerializeField]
    private Vector2 playerStartPosition = new Vector2(0f,-6.5f);     //Starting position for player
    
    //Score
    public int score = 0;                                          //Current Player Score
    // score Property
    public int Score
    {
        get
        {
            return score;
        }
    }

    private bool gameOver = false;
    public bool GameOverBool
    {
        get
        {
            return gameOver;
        }
    }

    private bool paused = false;

    //Delegate for those who care about the Score
    public delegate void ScoreAdded(int score);
    public ScoreAdded onScoreAdded;

    private PowerUpManager powerUpManager;
    public PowerUpManager PowerUpManager
    {
        get
        {
            return powerUpManager;
        }
    }

    //Delegate for when player takes damage
    public delegate void PlayerTookDamage(float health);
    public PlayerTookDamage onPlayerTookDamage;

    private void Awake()
    {
        //Check if singular instance
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        //Get reference to Player object
        activePlayer = Instantiate<GameObject>(player, playerStartPosition, Quaternion.identity);
        powerUpManager = gameObject.GetComponent<PowerUpManager>();
    }

    private void Start()
    {
        this.gameOver = false;
        playerHealth = activePlayer.gameObject.GetComponent<PlayerHealth>();
        //Subscribe to when Player Takes damage
        playerHealth.onPlayerTakesDamage += NotifyHealthBarUI;
        playerShooter = activePlayer.gameObject.GetComponent<PlayerShooter>();

        ResetScore();
    }

    public void NotifyHealthBarUI()
    {
        if (onPlayerTookDamage != null)
        {
            onPlayerTookDamage((float)playerHealth.Health / (float)playerHealth.StartHealth);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        
    }

    private void PauseGame()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0.0f;
            Debug.Log("Paused");
        }
        else
        {
            paused = false;
            Time.timeScale = 1.0f;
            Debug.Log("Unpaused");
        }
    }

    public void GameOver()
    {
        SaveScore();
        gameOver = true;
        LevelManager.LoadGameOver(); // Is this a good?
    }

    /******************************   Score Manager Section   ******************************/

    /// <summary>
    /// Resets the score to 0
    /// </summary>
    private void ResetScore()
    {
        this.score = 0;
    }

    /// <summary>
    /// Adds value to the total score in ScoreManager
    /// </summary>
    /// <param name="scoreToAdd">int</param>
    public void AddScore(int scoreToAdd)
    {
        this.score += scoreToAdd;

        if (onScoreAdded != null) onScoreAdded(this.score);
    }

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Current Score", this.score);

        for (int i = 1; i <= 5; i++)
        {
            int highScore = PlayerPrefs.GetInt("HS" + i, 0);
            if (this.score > highScore)
            {
                for (int j = 5; j > i; j--)
                {
                    PlayerPrefs.SetInt("HS" + (j), PlayerPrefs.GetInt("HS" + (j - 1), 0));
                }
                PlayerPrefs.SetInt("HS" + i, this.score);
                return;
            }
        }
    }

    /******************************   END Score Manager Section END   ******************************/

    /******************************   START ObjectPool Helper **************************************/
    private IEnumerator DestroyGarbageGameObjects(List<GameObject> deleteList, float time)
    {
        yield return new WaitForSeconds(time);
        foreach (GameObject obj in deleteList)
        {
            Destroy(obj);
        }
    }

    public void CleanList(List<GameObject> list)
    {
        if (!gameOver)
        {
            StartCoroutine(DestroyGarbageGameObjects(list, 2.0f));
        }
    }
}