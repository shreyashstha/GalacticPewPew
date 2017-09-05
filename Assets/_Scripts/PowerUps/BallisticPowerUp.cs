using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticPowerUp : ActivatablePowerUp {

    [SerializeField]
    private GameObject projectile;

    public override void ExecutePowerUp()
    {
        Instantiate(projectile, GameManager.instance.m_Player.transform.position, Quaternion.identity);
        DeactivatePowerUp();
    }
}
