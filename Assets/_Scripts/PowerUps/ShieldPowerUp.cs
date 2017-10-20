using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : ActivatablePowerUp
{
    public int hits = 0;        //Number of hits to absorb
    public GameObject shieldDownParticle;  //Particle to emit when shield is down
    public Sprite[] shieldSprite;       //Different sprites representing different states of the shield
    [SerializeField]
    private bool timed = false;         //True if shield goes down after some time
    [SerializeField]
    private float upTime = 0.0f;        //Shield up time
    private bool executed = false;      //True if shield is activated

    protected override void Update()
    {
        base.Update();
        if (executed)
        {
            this.transform.position = GameManager.instance.m_Player.transform.position + relativePosition;
        }
    }

    public override void ExecutePowerUp()
    {
        executed = true;
        //Set tag to shield. Note done here to remove bug of destroying dropping shield
        gameObject.tag = "Shield";

        //If timed start coroutine
        if (timed)
        {
            StartCoroutine(ShieldTimerCoroutine(upTime));
        }
        else
        {
            //Delete other active shields
            GameObject[] copies = GameObject.FindGameObjectsWithTag(this.tag);
            for (int i = 0; i < copies.Length; i++)
            {
                if (!this.gameObject.Equals(copies[i])) Destroy(copies[i]);
            }
            _spriteRenderer.sprite = equipSprite;
            _spriteRenderer.enabled = true;
            _collider.enabled = true;
            GameManager.instance.PlayerScript.Vulnerable = false;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        //Take damage if active
        if (active)
        {
            if (collision.gameObject.tag == "EnemyProjectile" && !timed)
            {
                TakeDamage();
            }
        }
    }

    IEnumerator ShieldTimerCoroutine(float time)
    {
        ShieldSetup();
        yield return new WaitForSeconds(time);
        ShieldTearDown();
    }

    private void TakeDamage()
    {
        hits--;
        if (hits == 0)
        {
            ShieldTearDown();
        } else
        {
            _spriteRenderer.sprite = shieldSprite[shieldSprite.Length - hits];
        }
    }

    /// <summary>
    /// Creates a particle emitter and destroys it after 0.3 secs
    /// </summary>
    private void EmitParticles(float time)
    {
        GameObject newParticle = Instantiate(shieldDownParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, time);
    }

    /// <summary>
    /// Setup steps for shield
    /// Enable SpriteRenderer and Collider2D
    /// Flip player vulnerable bool
    /// change layer to Player layer
    /// NOTE: Made a function because it is repeated twice.
    /// </summary>
    private void ShieldSetup()
    {
        _spriteRenderer.sprite = equipSprite;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        GameManager.instance.PlayerScript.Vulnerable = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// Teardown steps for shield
    /// Emit ShieldDown Particles
    /// Flip player vulnerable bool
    /// Deactivate power up function
    /// </summary>
    private void ShieldTearDown()
    {
        EmitParticles(0.5f);
        GameManager.instance.PlayerScript.Vulnerable = true;
        DeactivatePowerUp();
    }

    private void OnDestroy()
    {

    }
}
