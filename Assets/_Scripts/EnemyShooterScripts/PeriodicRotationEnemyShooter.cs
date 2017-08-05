using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicRotationEnemyShooter : EnemyShooter {

    private const float _RADIUS = 0.7f;

    [SerializeField]
    private int numberOfShots;
    [SerializeField]
    private float intervalBetweenShots = 1.0f;
    [SerializeField]
    private float arcAngle = 90f;

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
        if (numberOfShots <= 0) numberOfShots = 1;
        float angle = arcAngle / (numberOfShots - 1);
        for (int i = 0; i < numberOfShots; i++)
        {
            float startAngle = 0 - (arcAngle / 2);
            Quaternion rotation = Quaternion.AngleAxis(startAngle + angle * i, Vector3.forward);
            Debug.Log(rotation.eulerAngles.x + "  " + rotation.eulerAngles.y + "  " + rotation.eulerAngles.z);
            InstantiateProjectiles(rotation);
            yield return new WaitForSeconds(intervalBetweenShots);
        }
    }

    private void InstantiateProjectiles(Quaternion rotation)
    {
        GameObject spawnObject = pool._OBP_GetPooledObject();
        if (spawnObject != null)
        {
            spawnObject.transform.rotation = rotation;
            spawnObject.transform.position = this.transform.position + (rotation * new Vector3 (0, _RADIUS, 0));
            spawnObject.SetActive(true);
        }
        else
        {
            Debug.Log("EnemyShooter null pool");
        }
    }
}
