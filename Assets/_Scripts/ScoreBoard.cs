using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

    //High Score Text UI Elements
    public Text hs1;
    public Text hs2;
    public Text hs3;
    public Text hs4;
    public Text hs5;

    // Use this for initialization
    void Start () {
        hs1.text = "1.  " + PlayerPrefs.GetInt("HS1", 0);
        hs2.text = "2.  " + PlayerPrefs.GetInt("HS2", 0);
        hs3.text = "3.  " + PlayerPrefs.GetInt("HS3", 0);
        hs4.text = "4.  " + PlayerPrefs.GetInt("HS4", 0);
        hs5.text = "5.  " + PlayerPrefs.GetInt("HS5", 0);
    }
}
