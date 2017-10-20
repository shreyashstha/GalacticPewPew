using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrop : MonoBehaviour {

    private Transform player;
    [SerializeField]
    private float speed = 8.0f;

    // Use this for initialization
    void Start () {
        player = GameManager.instance.m_Player.transform;
    }
	
	// Update is called once per frame
	void Update () {
        MoveTowardsPlayer();
	}

    private void MoveTowardsPlayer()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);
    }
}
