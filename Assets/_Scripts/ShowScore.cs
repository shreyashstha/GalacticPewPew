using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour {

    private Text scoreBoard;

    // Use this for initialization
    void Start () {
        scoreBoard = this.gameObject.GetComponent<Text>();
        scoreBoard.text = ""+PlayerPrefs.GetInt("Current Score");
	}

}
