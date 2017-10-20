using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUIUpdater : MonoBehaviour {

    private Text scoreBoard;

    // Use this for initialization
	void Start () {
        scoreBoard = this.gameObject.GetComponent<Text>();

        //Set start score to 0
        DisplayScore(0);
        //Subscribe to OnScoreAdded
        GameManager.instance.onScoreAdded += DisplayScore;
    }

    /// <summary>
    /// Displays the current total score in the UI
    /// </summary>
    void DisplayScore(int score)
    {
        scoreBoard.text = "" + score;
    }
}
