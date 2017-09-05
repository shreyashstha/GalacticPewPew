using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPowerUp : EquippablePowerUp {

    private PlayerShooter[] shooters;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        shooters = gameObject.GetComponents<PlayerShooter>();
        _animator = gameObject.GetComponent<Animator>();
        _animator.enabled = false;
        shooters[0].onPlayerIsShooting += TriggerAnimation;
    }

    public override void ExecutePowerUp()
    {
        gameObject.tag = "PlayerShooter";
        _animator.enabled = true;
        GameManager.instance.PlayerScript.ChangeShooter(this.transform);
        transform.localPosition = relativePosition;
        foreach (PlayerShooter shooter in shooters)
        {
            shooter.enabled = true;
        }
    }

    void TriggerAnimation()
    {
        _animator.SetTrigger("Shoot");
    }
}
