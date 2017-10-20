using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PenetrateProjectile : Projectile {

    [SerializeField]
    private int maxHitCount = 3;
    private int hitcount = 0;

    public override void Hit()
    {
        hitcount++;
        if (hitcount == maxHitCount)
        {
            this.gameObject.SetActive(false);
        }
    }

    public override void OnDisable()
    {
        hitcount = 0;
    }
}
