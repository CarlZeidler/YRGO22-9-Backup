using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float spawnTime = 2f;
    public Vector3 spawnPoint;
    [SerializeField] private ParticleSystem travelParticles, deathParticles;
    [SerializeField] private GameObject textObjectOnDeath;

    [Space(20)]
    //scripts to disable
    [SerializeField] private Collider2D interactionCollider, physicsCollider;
    [SerializeField] private GameObject playerVisuals;
    [SerializeField] private PlayerMove pMove;
    [SerializeField] private PlayerHack pHack;
    [SerializeField] private Rigidbody2D rb;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Respawn();
        }
    }
    public void Respawn()
    {
        textObjectOnDeath.SetActive(false);
        textObjectOnDeath.GetComponent<Animator>().SetTrigger(0);

        //play particles
        //travelParticles.startLifetime = spawnTime;
        var particles = travelParticles.main;
        particles.simulationSpeed = Mathf.Pow(spawnTime, -1);
        travelParticles.Play();
        //lerp pos to spawnpoint then enable scripts
        StartCoroutine(TravelToSpawnPoint(spawnTime));

        //DISABLE HACKINGMODE
        if (pHack.inHackingMode)
            pHack.ToggleHackingMode();
        //RESETHACKSTATES
        GameManager.instance.ResetHackables();
        //energy
        pHack.ResetCharges();
    }
    public void Die()
    {        
        //disable all relevant scripts
        EnableScripts(false);

        deathParticles.Play();

        textObjectOnDeath.SetActive(true);
    }
    private void EnableScripts(bool enable)
    {
        if (!enable)
        {
            interactionCollider.enabled = false;
            physicsCollider.enabled = false;
            //pMove.enabled = false;
            pMove.canMove = false;
            pHack.enabled = false;
            playerVisuals.SetActive(false);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        else
        {
            interactionCollider.enabled = true;
            physicsCollider.enabled = true;
            //pMove.enabled = true;
            pMove.canMove = true;
            pHack.enabled = true;
            playerVisuals.SetActive(true);
            rb.velocity = Vector2.zero;
            rb.isKinematic = false;
        }
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
        Invoke(nameof(Spawn),spawnTime/5);
    }
}
