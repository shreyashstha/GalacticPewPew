using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrop : MonoBehaviour {

    public GameObject powerUp;
    private Transform player;
    [SerializeField]
    private float speed = 8.0f;

    // Use this for initialization
    void Start () {
        player = GameManager.instance.Player.transform;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AttachPowerUp();
            DisableDrop();
        }
    }

    private void AttachPowerUp()
    {
        Instantiate(this.powerUp, player.position, Quaternion.identity);
        //powerUp.transform.SetParent(player);
    }

    private void DisableDrop()
    {
        Destroy(this.gameObject);
    }
}
