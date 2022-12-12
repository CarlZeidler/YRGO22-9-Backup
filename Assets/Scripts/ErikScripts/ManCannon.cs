using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManCannon : MonoBehaviour
{
    public Rigidbody2D character;
    public Transform shootingPart;
    public bool manualControl;
    public float rotatingSpeed = 100f;
    public float shotForce = 50f;

    private bool isShooting = false;
    private float backColliderDelay = 0.05f;


    public void Start()
    {

    }


    public void Update()
    {

        if (Input.GetButtonDown("Fire1") && isShooting == true)
        {

            StartCoroutine("Shooting");

            print("Fired");

        }
    }

    IEnumerator Shooting()
    {

        character.transform.position = transform.position;
        character.transform.rotation = shootingPart.rotation;

        character.AddForce(transform.forward * shotForce, ForceMode2D.Impulse);

        character.transform.parent = GameObject.FindWithTag("Player").transform;
        character.GetComponent<MeshRenderer>().enabled = true;
        character.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        character.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(backColliderDelay);

        character.GetComponent<Collider>().enabled = true;

        isShooting = false;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isShooting = true;

            other.gameObject.transform.parent = transform;
            other.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            other.gameObject.transform.localRotation = Quaternion.identity;
            other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        if (manualControl == true)

        {
            // Move to the right
            if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.Rotate(Vector3.up * rotatingSpeed * Time.deltaTime);
            }

            // Move to the left
            else if (Input.GetButton("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.Rotate(Vector3.down * rotatingSpeed * Time.deltaTime);
            }

            // Move to the up
            else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") < 0)
            {
                transform.Rotate(Vector3.right * rotatingSpeed * Time.deltaTime);
            }

            // Move to the down
            else if (Input.GetButton("Vertical") && Input.GetAxisRaw("Vertical") > 0)
            {
                transform.Rotate(Vector3.left * rotatingSpeed * Time.deltaTime);
            }
        }
    }
}