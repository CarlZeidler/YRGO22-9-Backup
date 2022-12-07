using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    public Vector2 spawnPoint;
    public void SetSpawn()
    {
        if (spawnPoint != null)
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = spawnPoint;
        else
            GameManager.instance.player.GetComponent<PlayerRespawn>().spawnPoint = transform.position;

        particles.Play();
    }
}
