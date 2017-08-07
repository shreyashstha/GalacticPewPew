using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp {


    public int hits = 0;

    public override void EnablePowerUp()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage();
        };
    }

    private void TakeDamage()
    {
        hits--;
        if (hits == 0)
        {
            DisablePowerUp();
        }
    }
}
