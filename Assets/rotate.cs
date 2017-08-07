using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    Vector3 center;
    public float speed = 1.0f;
    // Use this for initialization
	void Start () {
        center = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = this.transform.position - this.center;
        newPos = Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.forward) * newPos;
        this.transform.position = this.center + newPos;
    }
}
