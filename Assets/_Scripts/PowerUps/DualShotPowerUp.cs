using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualShotPowerUp : PowerUp {

    [SerializeField]
    private float time = 5.0f;

    public override void EnablePowerUp()
    {
        StartCoroutine("DisableAfterSomeTime",time);
    }

    IEnumerator DisableAfterSomeTime(float time)
    {
        yield return new WaitForSeconds(time);
        DisablePowerUp();
    }
}
