using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour {

    private Image image;
    private Sprite startSprite;

	// Use this for initialization
	void Start () {
        image = gameObject.GetComponent<Image>();
        GameManager.instance.onPowerUpChange += UpdateImage;
        startSprite = image.sprite;
	}

    void UpdateImage(Sprite sprite)
    {
        if (sprite == null)
        {
            image.sprite = startSprite;
        }
        else
        {
            image.sprite = sprite;
        }
    }
}
