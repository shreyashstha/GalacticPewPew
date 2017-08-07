using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShooterEnemy : MonoBehaviour, IEnemyShooter {

    public Transform barrelOne, barrelTwo;
    public GameObject projectile;
    public float initialDelay = 1.0f;
    public float attackSpeed = 2.0f;
    public int chanceToShoot = 0;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Shoot", initialDelay, attackSpeed);
    }

    public void Shoot()
    {
        int random = Random.Range(0, 99);
        if (random < chanceToShoot)
        {
            Instantiate(projectile, barrelOne.position, Quaternion.identity);
            Instantiate(projectile, barrelTwo.position, Quaternion.identity);
        }
    }
}
