﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPoolableObject {

    public float speed;
    public AudioClip hitClip;       //sound to play when hit by player
    private Rigidbody2D rigidBody2D;
    private SpriteRenderer spriteRenderer;
    private int hits = 4;

    private void Awake()
    {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Boundary.OutOfBoundary(transform.position, 3f))
        {
            this.gameObject.SetActive(false);
        }
    }

    void Hit()
    {
        hits--;
        AudioSource.PlayClipAtPoint(hitClip, transform.position);
        if (hits == 0)
        {
            StopAllCoroutines();
            GameManager.instance.PowerUpManager.DropPowerUp(this.transform.position);
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine("Flash");
        }


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerProjectile")
        {
            Hit();
        }
    }

    //******Object Pooling******
    public void OnEnable()
    {
        spriteRenderer.material.SetFloat("_MaskAmount", 0.0f);
        hits = 4;
        Vector2 force;
        if (!InRightQuadrant())
        {
            force = new Vector2((float)Random.Range(7, 10) * 10f, (float)Random.Range(-1, 3) * 10f);
        }else
        {
            force = new Vector2(-(float)Random.Range(7, 10) * 10f, (float)Random.Range(-1, 3) * 10f);
        }
        rigidBody2D.AddForce(force);
    }

    public void OnDisable()
    {
        
    }
    //******End Object Pooling******

    /// <summary>
    /// Creates a Flash effect
    /// </summary>
    /// <returns></returns>
    IEnumerator Flash()
    {
        float min = 0.0f;
        float max = 1.0f;
        float t = 0.0f;

        spriteRenderer.material.SetColor("_MaskColor", new Color(1, 1, 1, 1));

        while (t < 1.0f)
        {
            t += 0.15f;

            float amount = Mathf.Lerp(min, max, t);
            spriteRenderer.material.SetFloat("_MaskAmount", amount);
            yield return new WaitForEndOfFrame();
        }

        spriteRenderer.material.SetFloat("_MaskAmount", 0.0f);
    }

    /// <summary>
    /// Returns true if located in the right quadrant of the game screen, false othewise
    /// </summary>
    /// <returns>boolean</returns>
    protected bool InRightQuadrant()
    {
        if (this.transform.position.x > 0)
        {
            return true;
        }
        else if (this.transform.position.x < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
