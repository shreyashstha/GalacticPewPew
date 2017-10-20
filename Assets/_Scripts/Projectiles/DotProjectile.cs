using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DotProjectile : ExplodeProjectile {

	public override IEnumerator ExplodeCoroutine()
    {
        //Store original radius
        float radius = ((CircleCollider2D)_collider2D).radius;
        float duration = 0f;
        while (duration < explosionDuration)
        {
            duration += 0.5f;
            ((CircleCollider2D)_collider2D).radius = explosionRadius;
            yield return new WaitForSeconds(0.2f);
            ((CircleCollider2D)_collider2D).radius = 0f;
            yield return new WaitForSeconds(0.3f);
        }
        //Revert radius
        ((CircleCollider2D)_collider2D).radius = radius;
        Destroy(this.gameObject);
    }
}
