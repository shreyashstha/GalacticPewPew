using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour {

    private Image image;

	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
        GameManager.instance.onPowerUpChange += UpdateImage;
	}

    void UpdateImage(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
