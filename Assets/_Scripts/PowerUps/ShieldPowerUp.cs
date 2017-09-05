using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : ActivatablePowerUp
{
    public int hits = 0;
    public GameObject hitParticle;
    public Sprite[] shieldSprite;
    private bool executed = false;

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
        GameObject[] copies = GameObject.FindGameObjectsWithTag(this.tag);
        for (int i = 0; i < copies.Length; i++)
        {
            if (!this.gameObject.Equals(copies[i])) Destroy(copies[i]);
        }
        _spriteRenderer.sprite = equipSprite;
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        GameManager.instance.PlayerScript.ToggleVulnerability();
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (active)
        {
            if (collision.gameObject.tag == "EnemyProjectile")
            {
                TakeDamage();
            }
        }
    }

    private void TakeDamage()
    {
        hits--;
        if (hits == 0)
        {
            GameManager.instance.PlayerScript.ToggleVulnerability();
            EmitParticles(0.5f);
            DeactivatePowerUp();
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
        GameObject newParticle = Instantiate(hitParticle, transform.position, Quaternion.identity);
        Destroy(newParticle, time);
    }
}
