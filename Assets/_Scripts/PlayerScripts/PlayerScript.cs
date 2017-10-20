using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    
    [SerializeField]
    private GameObject startShooter;
    [SerializeField]
    private GameObject shield;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private PlayerHealth _playerHealthScript;
    private GameObject _playerShooter;
    private GameObject _playerShield;
    private bool noNeedToReset = false;
    private bool vulnerable = true;

    public bool Vulnerable
    {
        get
        {
            return vulnerable;
        }

        set
        {
            vulnerable = value;
            Debug.Log(vulnerable);
        }
    }

    // Use this for initialization
    void Start () {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        ResetShooter();
        _playerHealthScript = gameObject.GetComponent<PlayerHealth>();
        _playerHealthScript.onPlayerTakesDamage += ResetShooter;
        _playerShield = Instantiate(shield, transform.position, Quaternion.identity);
        _playerShield.SetActive(false);
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
        if (this._playerShooter != null) Destroy(this._playerShooter);
        shooter.parent = this.transform;
        this._playerShooter = shooter.gameObject;
        noNeedToReset = false;
    }

    public void AddHealthToPlayer()
    {
        _playerHealthScript.AddHealth();
    }

    public void ActivateShield()
    {
        _playerShield.SetActive(true);
    }
}
