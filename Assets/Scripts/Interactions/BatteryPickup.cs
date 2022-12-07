using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public int charge=5;

    public void AddCharge()
    {
        GameManager.instance.player.GetComponent<PlayerHack>().DrainCharge(-charge);
        GetComponent<ParticleSystem>().Play();
    }
    public void Respawn()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
