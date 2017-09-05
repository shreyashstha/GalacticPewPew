using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatablePowerUp : PowerUp {

    public Sprite buttonSprite;
    public Sprite equipSprite;

    public override void ActivatePowerUp()
    {
        _dropEmission.Stop();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        GameManager.instance.onExecutePowerUp = ExecutePowerUp;
        GameManager.instance.DelegatePowerUpChange(buttonSprite);
    }

    public override void DeactivatePowerUp()
    {
        GameManager.instance.onExecutePowerUp -= ExecutePowerUp;
        Destroy(this.gameObject);
    }
}
