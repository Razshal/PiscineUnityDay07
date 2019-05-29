﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanonScript : MonoBehaviour {
    public float rotationSpeed = 1;
    public GameObject rotaryParticle;
    public GameObject missileParticle;
    public GameObject canonParticle;
    public AudioClip gunShoot;
    public int gunDamages = 1;
    public int missileDamages = 10;
    public int fireRange = 20;
    public int missiles = 10;
    public float rotaryFireRate = 0.1f;
    public bool isPlayerCanon = false;
    public bool isFiring = false;
    private Vector3 direction;
    private RaycastHit raycastHit;
    private AudioSource audioSource;
    public Image cursor;

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
            if (raycastHit.collider.gameObject.CompareTag("Tanks"))
            {
				raycastHit.collider.gameObject.GetComponent<TankScript>().GetDamages(gunDamages);
                if (isPlayerCanon)
                    cursor.color = Color.red;
            }
        }
    }
    // TODO : refacto these two for less code repeat
    public void FireMissile()
    {
        if (missiles > 0)
        {
            CanonFireEffect(null);
            if (Physics.Raycast(transform.position, -transform.right, out raycastHit, fireRange))
            {
                Instantiate(missileParticle, raycastHit.point, transform.rotation);
                if (raycastHit.collider.gameObject.CompareTag("Tanks"))
                {
                    raycastHit.collider.gameObject.GetComponent<TankScript>().GetDamages(missileDamages);
                    if (isPlayerCanon)
                        cursor.color = Color.red;
                }
            }
            missiles--;
        }
    }

    public void StartRotary()
    {
        InvokeRepeating("FireRotary", 0, rotaryFireRate);
        isFiring = true;
    }

    public void StopRotary()
    {
        CancelInvoke("FireRotary");
        isFiring = false;
    }

	private void Start()
	{
        audioSource = gameObject.GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
        if (isPlayerCanon)
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0);
	}

	private void Update()
	{
        if (isPlayerCanon)
        {
            if (Input.GetMouseButtonDown(0))
                StartRotary();
            else if (Input.GetMouseButtonUp(0))
                StopRotary();

            if (Input.GetMouseButtonDown(1))
                FireMissile();
            if (isPlayerCanon)
                cursor.color = Color.white;
        }
	}
}
