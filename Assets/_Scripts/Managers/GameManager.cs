using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //*****Singleton*****
    public static GameManager instance = null;                      //Singleton instance

    //Player
    [SerializeField]
    private GameObject player;          //Player Prefab.

    //NOTE: 'm_' short for my.
    private GameObject m_activePlayer;    //Player GameObject in the scene.
    public GameObject m_Player            //activePlayer property
    {
        get
        {
            return m_activePlayer;
        }
    }

    private PlayerHealth m_playerHealth;      //PlayerHealth component of the Player gameobject

    private PlayerShooter m_playerShooter;    //PlayerShooter component of the Player gameobject
    public PlayerShooter m_PlayerShooter
    {
        get
        {
            return m_playerShooter;
        }
    }

    [SerializeField]
    private Vector2 m_playerStartPosition = new Vector2(0f,-6.5f);     //Starting position for player
    
    //Score
    public int score = 0;       //Current Player Score
    public int Score            //score Property
    {
        get
        {
            return score;
        }
    }

    private bool gameOver = false;  //Boolean true if player is dead. Game is over.
    public bool GameOverBool        //gameOver property
    {
        get
        {
            return gameOver;
        }
    }

    private bool paused = false;    //TODO: Pause functionality

    private PowerUpManager powerUpManager;      //PowerUpManager - has functions to when to spawn powerups.
    public PowerUpManager PowerUpManager        //PowerUpManager property
    {
        get
        {
            return powerUpManager;
        }
    }

    //***** Delegates *****
    //Delegate for those who care about the Score
    public delegate void ScoreAdded(int score);
    public ScoreAdded onScoreAdded;

    //Delegate for when player takes damage
    public delegate void PlayerTookDamage(float health);
    public PlayerTookDamage onPlayerTookDamage;
    //***** Delegates *****


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
        m_activePlayer = Instantiate<GameObject>(player, m_playerStartPosition, Quaternion.identity);
        powerUpManager = gameObject.GetComponent<PowerUpManager>();
    }

    private void Start()
    {
        this.gameOver = false;
        m_playerHealth = m_activePlayer.gameObject.GetComponent<PlayerHealth>();
        //Subscribe to when Player Takes damage
        m_playerHealth.onPlayerTakesDamage += DelegatePlayerTookDamage;
        m_playerShooter = m_activePlayer.gameObject.GetComponent<PlayerShooter>();

        ResetScore();
    }

    public void DelegatePlayerTookDamage()
    {
        if (onPlayerTookDamage != null)
        {
            onPlayerTookDamage((float)m_playerHealth.Health / (float)m_playerHealth.StartHealth);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
        
    }

    /// <summary>
    /// Pauses and Unpauses the game. Controlled by a button in the UI. TODO: Handle showing ui elements here.
    /// </summary>
    public void PauseGame()
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

    //DANGER:
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