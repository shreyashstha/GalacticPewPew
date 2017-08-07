using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour {

	//public Text scoreBoard;
	private int score = 0;

	public static ScoreManager instance = null;

    public int Score
    {
        get
        {
            return score;
        }
    }

    void Awake ()
	{
		if (instance != null) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}

		ResetScore();
		//DisplayScore();
	}

	// Use this for initialization
	void Update () {
		//DisplayScore();
	}
	
    /// <summary>
    /// Adds value to the total score in ScoreManager
    /// </summary>
    /// <param name="scoreToAdd">int</param>
	public void AddScore (int scoreToAdd)
	{
		this.score += scoreToAdd;
	}

    /// <summary>
    /// Displays the current total score in the UI
    /// </summary>
	//void DisplayScore(){
	//	scoreBoard.text = "SCORE "+score;
	//}

    /// <summary>
    /// Resets the score to 0
    /// </summary>
	public void ResetScore ()
	{
		this.score = 0;
        //DisplayScore();
	}

    public void SaveScore()
    {
        PlayerPrefs.SetInt("Current Score", this.score);
        
        for (int i = 1; i <= 5; i++)
        {
            int highScore = PlayerPrefs.GetInt("HS"+i, 0);
            if (this.score > highScore)
            {
                for (int j = 5; j > i; j--)
                {
                    PlayerPrefs.SetInt("HS"+(j), PlayerPrefs.GetInt("HS"+(j-1),0));
                }
                PlayerPrefs.SetInt("HS"+i, this.score);
                return;
            }
        }
    }
}
