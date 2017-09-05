using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    [SerializeField]
    private GameObject startShooter;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private GameObject shooter;
    private bool noNeedToReset = true;
    private bool vulnerable = true;

    public bool Vulnerable
    {
        get
        {
            return vulnerable;
        }
    }

    // Use this for initialization
    void Start () {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        shooter = transform.GetChild(0).gameObject;
        gameObject.GetComponent<PlayerHealth>().onPlayerTakesDamage += ResetShooter;        
    }

    public void ResetShooter()
    {
        if (!noNeedToReset)
        {
            GameObject shooter = Instantiate(startShooter, transform.position, Quaternion.identity);
            ChangeShooter(shooter.transform);
            noNeedToReset = true;
        }
    }

    public void ChangeShooter(Transform shooter)
    {
        Destroy(this.shooter);
        shooter.parent = this.transform;
        this.shooter = shooter.gameObject;
        noNeedToReset = false;
    }

    public void ToggleVulnerability()
    {
        vulnerable = !vulnerable;
    }
}
