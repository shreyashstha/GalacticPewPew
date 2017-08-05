﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASPowerUp : PowerUp {

    [SerializeField]
    private float attackSpeedMultiplier = 0.0f;
    [SerializeField]
    private float time = 0.0f;
    private bool attackSpeedReset = true;

    public override void EnablePowerUp()
    {
        StartCoroutine(IncreaseAttackSpeed(attackSpeedMultiplier,time));
    }

    IEnumerator IncreaseAttackSpeed(float multiplier, float time)
    {
        attackSpeedReset = false;
        GameManager.instance.PlayerShooter.AttackSpeed *= multiplier;
        yield return new WaitForSeconds(time);
        GameManager.instance.PlayerShooter.AttackSpeed /= multiplier;
        attackSpeedReset = true;
        DisablePowerUp();
    }

    private void OnDestroy()
    {
        if (!attackSpeedReset)
        {
            Debug.Log("I was deleted before reset so I reset on Destroy");
            GameManager.instance.PlayerShooter.AttackSpeed /= attackSpeedMultiplier;
        }
    }

}
