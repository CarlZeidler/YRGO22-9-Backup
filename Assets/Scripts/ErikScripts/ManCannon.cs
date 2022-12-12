using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManCannon : MonoBehaviour
{
    [SerializeField] private Rigidbody player;
    [SerializeField] private Transform cannonPart;
    public bool manualControl;
    [Range(15, 100)] private float rotatingSpeed = 100f;
    public float launchForce = 50f;

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

        player.transform.position = transform.position;
        player.transform.rotation = cannonPart.rotation;

        player.AddForce(transform.forward * launchForce, ForceMode.VelocityChange);

        player.transform.parent = GameObject.FindWithTag("Player").transform;
        player.GetComponent<MeshRenderer>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        player.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(backColliderDelay);

        player.GetComponent<Collider>().enabled = true;

        isShooting = false;

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            other.gameObject.transform.parent = transform;
            other.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            other.gameObject.transform.localRotation = Quaternion.identity;
            other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;

            isShooting = true;

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