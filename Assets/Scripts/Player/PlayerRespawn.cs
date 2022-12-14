using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerRespawn : MonoBehaviour
{
    public float spawnTime = 2f;
    public Vector3 spawnPoint;
    [SerializeField] private ParticleSystem travelParticles, deathParticles;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private GameObject textObjectOnDeath;

    [Space(20)]
    //scripts to disable
    [SerializeField] private Collider2D interactionCollider, physicsCollider;
    [SerializeField] private GameObject playerVisuals;
    [SerializeField] private PlayerMove pMove;
    [SerializeField] private PlayerHack pHack;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ShadowCaster2D shadowCaster;

    public float respawnHold = 0f;


    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && respawnHold >= 1f)
        {
            Respawn();
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            respawnHold += 1 * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
            respawnHold = 0f;
    }
    public void Respawn()
    {
        deathSound.Play();
        //GameManager.instance.composer.Stop();
        try
        {
            GameManager.instance.composer.Inverse();
        }
        catch { Debug.Log("Lägg på jukebox stupid"); }
        //if die not triggered
        if (!textObjectOnDeath.activeSelf)
        {
            //disable all relevant scripts
            EnableScripts(false);

            //deathParticles.Play();

            textObjectOnDeath.SetActive(true);
        }
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
        GameManager.instance.ResetPickups();
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
        shadowCaster.enabled = enable;
        interactionCollider.enabled = enable;
        physicsCollider.enabled = enable;
        pMove.canMove = enable;
        pHack.enabled = enable;
        playerVisuals.SetActive(enable);
        rb.velocity = Vector2.zero;
        rb.isKinematic = !enable;
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
            respawnHold = 0f;
            yield return null;
        }
        transform.position = spawnPoint;

        GameManager.instance.composer.Stop();
        GameManager.instance.composer.ChangeLoop();
        GameManager.instance.composer.Inverse();
        Invoke(nameof(Spawn),spawnTime/5);
    }
}
