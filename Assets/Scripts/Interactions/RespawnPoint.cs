using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] GameObject respawnLight;
    public Transform spawnpoint;
    
    public void Start()
    {
        respawnLight.SetActive(false);
        GameManager.instance.respawnPoints.Add(this);
    }

    public void SetSpawn()
    {
        GameManager.instance.TurnOffRespawnLights();
        respawnLight.SetActive(true);
        if (spawnpoint != null)
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = spawnpoint.position;
        else
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = transform.position;

        particles.Play();
        GameManager.instance.player.GetComponent<PlayerHack>().ResetCharges();
    }

    public void TurnOffLight()
    {
        respawnLight.SetActive(false);
    }
}
