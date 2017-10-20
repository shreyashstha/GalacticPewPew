using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //*****Singleton*****
    public static GameManager instance = null;                      //Singleton instance

    [SerializeField]
    private GameObject player;          //Player Prefab.

    //NOTE: 'm_' short for my.
    private GameObject m_activePlayer;    //Player GameObject in the scene.
    private PlayerHealth m_playerHealth;      //PlayerHealth component of the Player gameobject
    private PlayerScript m_playerScript;      //PlayerScript component of the Player gameobject
    private PowerUpManager powerUpManager;    //PowerUpManager - has functions to when to spawn powerups.
    [SerializeField]
    private Vector2 m_playerStartPosition = new Vector2(0f,-7.5f);     //Starting position for player
    private bool gameOver = false;            //Boolean true if player is dead. Game is over.
    private bool paused = false;    //TODO: Pause functionality
    [SerializeField]
    private GameObject pauseMenu;   //The Pause Menu
    private int score = 0;      //Current Player Score

    //Random variables for various stuff
    private int healthPieceCount = 0;       //Number of health pieces collected for health button
    private int maxHealthPieces = 5;        //Mas health pieces needed to activate button
    public ButtonCoolDownFiller _healthPieceButton; //Health Piece button script
    private int shieldKillCount = 0;     //Number of enemies killed
    private int maxShieldKillCount = 15;    //Number of enemies to kill for shield
    public ButtonCoolDownFiller _shieldButton;  //Shield button script 

    public GameObject m_Player            //activePlayer property
    {
        get
        {
            return m_activePlayer;
        }
    }

    public int Score            //score Property
    {
        get
        {
            return score;
        }
    }

    public bool GameOverBool        //gameOver property
    {
        get
        {
            return gameOver;
        }
    }

    public bool Paused          //Paused property
    {
        get
        {
            return paused;
        }
    }

    public PowerUpManager PowerUpManager        //PowerUpManager property
    {
        get
        {
            return powerUpManager;
        }
    }

    public PlayerScript PlayerScript
    {
        get
        {
            return m_playerScript;
        }
    }

    //***** Delegates *****
    //Delegate for those who care about the Score
    public delegate void ScoreAdded(int score);
    public ScoreAdded onScoreAdded;

    //Delegate for when player takes damage
    public delegate void PlayerTookDamage(float health);
    public PlayerTookDamage onPlayerTookDamage;

    //Delegate for when health is added to player
    public delegate void PlayerAddedHealth(float health);
    public PlayerAddedHealth onPlayerAddedHealth;

    //Delegate for when Power up changes
    public delegate void PowerUpChange(Sprite sprite);
    public PowerUpChange onPowerUpChange;

    //Delegate for when Power Up button is pushed
    public delegate void ExecutePowerUp();
    public ExecutePowerUp onExecutePowerUp;
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
        //Instantiate Player object
        m_activePlayer = Instantiate<GameObject>(player, m_playerStartPosition, Quaternion.identity);
        powerUpManager = gameObject.GetComponent<PowerUpManager>();
    }

    private void Start()
    {
        this.gameOver = false;
        m_playerHealth = m_activePlayer.gameObject.GetComponent<PlayerHealth>();
        m_playerScript = m_activePlayer.gameObject.GetComponent<PlayerScript>();

        //Subscribe to when Player Takes damage
        m_playerHealth.onPlayerTakesDamage += DelegatePlayerTookDamage;
        m_playerHealth.onPlayerAddHealth += DelegatePlayerAddedHealth;

        //Reset the score at the start of the game
        ResetScore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }

    }

    /// <summary>
    /// Function that calls the delegate onPlayerTookDamage
    /// </summary>
    public void DelegatePlayerTookDamage()
    {
        if (onPlayerTookDamage != null)
        {
            onPlayerTookDamage((float)m_playerHealth.Health / (float)m_playerHealth.StartHealth);
        }
    }

    /// <summary>
    /// Function that calls the delegate onPlayerAddedHealth
    /// </summary>
    public void DelegatePlayerAddedHealth()
    {
        if (onPlayerTookDamage != null)
        {
            onPlayerTookDamage((float)m_playerHealth.Health / (float)m_playerHealth.StartHealth);
        }
    }

    /// <summary>
    /// Function that calls the delegate onPowerUpChange
    /// </summary>
    /// <param name="sprite"></param>
    public void DelegatePowerUpChange(Sprite sprite)
    {
        if (onPowerUpChange != null)
        {
            onPowerUpChange(sprite);
        }
    }

    /// <summary>
    /// Function that calls the delegate onExecutePowerUp
    /// </summary>
    public void DelegateExecutePowerUp()
    {
        if (onExecutePowerUp != null)
        {
            onExecutePowerUp();
        }
        DelegatePowerUpChange(null);
    }

    /// <summary>
    /// Pauses and Unpauses the game. Controlled by a button in the UI. TODO: Handle showing ui elements here.
    /// This is called by PauseButton
    /// </summary>
    public void TogglePause()
    {
        if (!paused)
        {
            paused = true;
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            paused = false;
            Time.timeScale = 1.0f;
            pauseMenu.SetActive(false);
        }
    }

    public void IncrementHealthPiece()
    {
        if (healthPieceCount < maxHealthPieces)
        {
            healthPieceCount++;
        }
        _healthPieceButton.UpdateFillCount((float)healthPieceCount / (float)maxHealthPieces);
    }

    public void ResetPlayerHealth()
    {
        if (healthPieceCount == maxHealthPieces)
        {
            m_playerScript.AddHealthToPlayer();
            healthPieceCount = 0;
            _healthPieceButton.UpdateFillCount((float)healthPieceCount / (float)maxHealthPieces);
        }
    }

    public void IncrementEnemyKills()
    {
        this.shieldKillCount++;
        if (shieldKillCount <= maxShieldKillCount) _shieldButton.UpdateFillCount((float)shieldKillCount / (float)maxShieldKillCount);
    }

    public void ActivatePlayerShield()
    {
        if (this.shieldKillCount >= this.maxShieldKillCount)
        {
            m_playerScript.ActivateShield();
            this.shieldKillCount = 0;
            _shieldButton.UpdateFillCount((float)shieldKillCount / (float)maxShieldKillCount);
        }
    }

    //**********Level Manager Functions**********
    /// <summary>
    /// Calls GameOver and tells Level Manager to restart game
    /// </summary>
    public void RestartGame()
    {
        TogglePause();
        GameOver();
        LevelManager.LoadGame();
    }

    /// <summary>
    /// Calls GameOver and tells Level Manager to restart game
    /// </summary>
    public void MainMenu()
    {
        TogglePause();
        GameOver();
        LevelManager.LoadMainMenu();
    }

    /// <summary>
    /// Saves the score and loads the GameOver scene
    /// This is only called by Player health script when health is 0
    /// </summary>
    public void GameOver()
    {
        Debug.Log(Time.time);
        SaveScore();
        gameOver = true;
        LevelManager.LoadGameOver(); // Is this a good?
    }
    //**********End Level Manager Functions**********

    //Calls the GarbageCollectCoroutine
    public void GarbageCollectPooledObjects(List<PooledObject> list)
    {
        if (GameManager.instance != null)
        {
            StartCoroutine(GarbageCollectCoroutine(list));
        }
    }

    /// <summary>
    /// Destroys each game object in the PooledObjects
    /// </summary>
    /// <param name="list">List of PooledObject from some object pool that has been destroyed</param>
    /// <returns></returns>
    IEnumerator GarbageCollectCoroutine(List<PooledObject> list)
    {
        yield return new WaitForSeconds(10f);

        foreach (PooledObject pool in list)
        {
            foreach (GameObject go in pool.pool)
            {
                Destroy(go);
            }
        }
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
}
 