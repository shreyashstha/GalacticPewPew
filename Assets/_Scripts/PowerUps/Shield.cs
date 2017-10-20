using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Shield : MonoBehaviour
{

    [SerializeField]
    private float upTime = 0.0f;        //Shield up time
    [SerializeField]
    private Vector3 relativePos;
    [SerializeField]
    private AudioClip hitClip;      //Audioclip to play when hit

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(ShieldTimerCoroutine(upTime));
    }

    private void OnEnable()
    {
        this.transform.position = GameManager.instance.m_Player.transform.position + relativePos;
        StartCoroutine(ShieldTimerCoroutine(upTime));
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = GameManager.instance.m_Player.transform.position + relativePos;
    }

    IEnumerator ShieldTimerCoroutine(float time)
    {
        //GameManager.instance.PlayerScript.Vulnerable = false;
        yield return new WaitForSeconds(time);
        //GameManager.instance.PlayerScript.Vulnerable = true;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            AudioSource.PlayClipAtPoint(hitClip, transform.position);
        }
    }
}
