using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolDownFiller : MonoBehaviour {

    private Image uiImage;

	// Use this for initialization
	void Start () {
        uiImage = gameObject.GetComponent<Image>();
        UpdateFillCount(0);
	}

    public void UpdateFillCount(float amount)
    {
        uiImage.fillAmount = amount;
    }
}
