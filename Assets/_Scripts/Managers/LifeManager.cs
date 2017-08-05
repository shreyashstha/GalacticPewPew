using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour {

    public static LifeManager instance = null;
    private float health = 0;
    private float totalHealth = 1;

    public float Health
    {
        get
        {
            return health;
        }

        set
        {
            health = (float)value;
        }
    }

    /// <summary>
    /// Health Property of LifeManager
    /// Displayed to the UI - Health Value
    /// </summary>
    public void SetTotalHealth(int value)
    {
        this.totalHealth = (float)value;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// Displays the health to the UI every frame
    /// </summary>
    public float CurrentHealthRatio()
    {
        return (float)(Health / totalHealth);
    }
}
