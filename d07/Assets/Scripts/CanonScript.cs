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
    public float rotaryFireRate = 0.1f;
    private Vector3 direction;
    private RaycastHit raycastHit;
    private AudioSource audioSource;

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void Fire()
    {
        canonParticle.SetActive(true);
        canonParticle.GetComponent<ParticleSystem>().time = 0;
        PlaySound(gunShoot);
        if (Physics.Raycast(transform.position, -transform.right, out raycastHit, fireRange))
        {
            Instantiate(rotaryParticle, raycastHit.point, transform.rotation);
        }
    }

	private void Start()
	{
        audioSource = gameObject.GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);
	}

	private void Update()
	{
        if (Input.GetMouseButtonDown(0))
            InvokeRepeating("Fire", 0, rotaryFireRate);
        else if (Input.GetMouseButtonUp(0))
            CancelInvoke("Fire");
	}
}
