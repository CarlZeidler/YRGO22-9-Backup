using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    public Transform spawnpoint;
    public void SetSpawn()
    {
        if (spawnpoint != null)
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = spawnpoint.position;
        else
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = transform.position;

        particles.Play();
        GameManager.instance.player.GetComponent<PlayerHack>().ResetCharges();
    }
}
