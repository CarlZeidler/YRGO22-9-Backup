using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardRespawn : MonoBehaviour
{
    public float spawnTime = 2f;
    public Vector3 spawnPoint;
    [SerializeField] private ParticleSystem travelParticles;
    [Space(20)]
    //disable
    [SerializeField] private Collider2D physicsCollider;
    [SerializeField] private GuardMove guardMove;
    [SerializeField] private GuardBehaviour guardBehaviour;
    [SerializeField] private GuardVision guardVision;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject visuals;


    private void Start()
    {
        spawnPoint = transform.position;
    }
    private void EnableScripts(bool enable)
    {
        physicsCollider.enabled = enable;
        guardMove.enabled = enable;
        guardBehaviour.enabled = enable;
        guardVision.enabled = enable;
        rb.isKinematic = !enable;
        if(!enable)
            rb.velocity = Vector2.zero;
        visuals.SetActive(enable);
    }
    public void Respawn()
    {
        EnableScripts(false);

        var particles = travelParticles.main;
        particles.simulationSpeed = Mathf.Pow(spawnTime, -1);
        travelParticles.Play();

        StartCoroutine(TravelToSpawnPoint(spawnTime));
    }
    public void Spawn()
    {
        //reenable scripts
        EnableScripts(true);

        //stop particles
        travelParticles.Stop();
        //travelParticles.enableEmission = false;
    }
    private IEnumerator TravelToSpawnPoint(float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Slerp(startPosition, spawnPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = spawnPoint;
        Invoke(nameof(Spawn), spawnTime / 5);
    }
}
