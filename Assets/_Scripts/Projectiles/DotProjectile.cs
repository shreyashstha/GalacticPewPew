using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProjectile : ExplodeProjectile {

	public override IEnumerator ExplodeCoroutine()
    {
        //Store original radius
        float radius = _circleCollider2D.radius;
        float duration = 0f;
        while (duration < explosionDuration)
        {
            duration += 0.5f;
            _circleCollider2D.radius = explosionRadius;
            yield return new WaitForSeconds(0.2f);
            _circleCollider2D.radius = 0f;
            yield return new WaitForSeconds(0.3f);
        }
        //Revert radius
        _circleCollider2D.radius = radius;
        Destroy(this.gameObject);
    }
}
