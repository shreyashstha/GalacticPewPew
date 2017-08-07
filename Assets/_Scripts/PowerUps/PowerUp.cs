using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour, IPowerUp {

    private Transform player;
    public Vector3 posRelativeToShip = new Vector3(0,0,0);

    // Base Start function
    /**
     * Get's a reference to Player GameObject in the Scene via GameManager
     * Collects other active gameObjects with the same tag as this object (NOTE: should I search instead using Script component?)
     * removes other powerups of this type
     * calls EnablePowerUp
     **/
    public virtual void Start () {
        player = GameManager.instance.m_Player.transform;
        GameObject[] otherPowerUpsInScene;
        otherPowerUpsInScene = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (otherPowerUpsInScene.Length > 0)
        {
            for (int i = 0; i < otherPowerUpsInScene.Length; i++)
            {
                if (otherPowerUpsInScene[i] != this.gameObject) Destroy(otherPowerUpsInScene[i]);
            }
        }
        EnablePowerUp();
    }

    // Update is called once per frame
    void Update () {
        this.transform.position = player.transform.position + posRelativeToShip;
	}

    public virtual void DisablePowerUp()
    {
        Destroy(this.gameObject);
    }

    public abstract void EnablePowerUp();
}
