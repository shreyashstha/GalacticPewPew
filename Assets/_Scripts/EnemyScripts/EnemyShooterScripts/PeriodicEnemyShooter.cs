using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicEnemyShooter : EnemyShooter {

    [SerializeField]
    private int numberOfShots;
    [SerializeField]
    private float intervalBetweenShots = 1.0f;

    public override void Shoot()
    {
        int random = Random.Range(0, 99);
        if (random < chanceToShoot)
        {
            StartCoroutine(ShootCoroutine (numberOfShots, intervalBetweenShots));
        }
    }

    IEnumerator ShootCoroutine(int numberOfShots, float intervalBetweenShots)
    {
        for (int i = 0; i < numberOfShots; i++)
        {
            InstantiateProjectiles();
            yield return new WaitForSeconds(intervalBetweenShots);
        }
    }
}
