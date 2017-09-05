using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] powerUps;

    public void DropPowerUp(Vector3 pos)
    {
        Instantiate(powerUps[Random.Range(0, powerUps.Length)], pos, Quaternion.identity);
    }
}
