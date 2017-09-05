using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquippablePowerUp : PowerUp {

    public Sprite equipSprite;

    public override void ActivatePowerUp()
    {
        _dropEmission.Stop();
        _spriteRenderer.sprite = equipSprite;
        _collider.enabled = false;
        ExecutePowerUp();
    }

    public override void DeactivatePowerUp()
    {
        Destroy(this.gameObject);
    }
}
