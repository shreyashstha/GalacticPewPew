using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour {

	private SpriteRenderer _spriteRenderer;
	private float min = 0.0f;
	private float max = 1.0f;
	private float t = 0.0f;

	// Use this for initialization
	void Start () {
		_spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		float amount = Mathf.Lerp (min, max, t);
		_spriteRenderer.material.SetFloat ("_MaskAmount", amount);

		t += 0.05f;

		if (t > 1.0f) {
			float temp = max;
			max = min;
			min = temp;
			t = 0.0f;
		}
	}
}
