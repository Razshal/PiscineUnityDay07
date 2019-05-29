using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonScript : MonoBehaviour {
    public float rotationSpeed = 1;
    public GameObject rotaryParticle;
    public GameObject missileParticle;
    public GameObject canonParticle;
    public AudioClip gunShoot;
    public int fireRange = 100;
    public int missiles = 10;
    public float rotaryFireRate = 0.1f;
    public bool isPlayerCanon = false;
    private Vector3 direction;
    private RaycastHit raycastHit;
    private AudioSource audioSource;


    private void CanonFireEffect(AudioClip clip)
    {
        canonParticle.SetActive(true);
        canonParticle.GetComponent<ParticleSystem>().time = 0;
        if (clip)
            audioSource.PlayOneShot(clip);
    }

    private void FireRotary()
    {
        CanonFireEffect(gunShoot);
        if (Physics.Raycast(transform.position, -transform.right, out raycastHit, fireRange))
        {
            Instantiate(rotaryParticle, raycastHit.point, transform.rotation);
        }
    }

    private void FireMissile()
    {
        CanonFireEffect(null);
        if (Physics.Raycast(transform.position, -transform.right, out raycastHit, fireRange))
            Instantiate(missileParticle, raycastHit.point, transform.rotation);
        missiles--;
    }

	private void Start()
	{
        audioSource = gameObject.GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
        if (isPlayerCanon)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);
        }
	}

	private void Update()
	{
        if (isPlayerCanon)
        {
            if (Input.GetMouseButtonDown(0))
                InvokeRepeating("FireRotary", 0, rotaryFireRate);
            else if (Input.GetMouseButtonUp(0))
                CancelInvoke("FireRotary");

            if (Input.GetMouseButtonDown(1) && missiles > 0)
                FireMissile();
        }
	}
}
