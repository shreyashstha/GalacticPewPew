using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIUpdater : MonoBehaviour {

    private Image healthBar;

	// Use this for initialization
	void Start () {
        healthBar = this.gameObject.GetComponent<Image>();
        UpdateHealth(1);

        GameManager.instance.onPlayerTookDamage += UpdateHealth;
        GameManager.instance.onPlayerAddedHealth += UpdateHealth;
    }

    /// <summary>
    /// Updates the HealthBar fill amount
    /// </summary>
    public void UpdateHealth(float fillAmount)
    {
        healthBar.fillAmount = fillAmount;
    }
}
