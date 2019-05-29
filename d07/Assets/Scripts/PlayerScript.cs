using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    private Vector3 movement;
    public float speed = 0.1f;
    public float rotationSpeed = 1;
    public float boostMultiplier = 2;
    public float boostTime;
    public float maxBoostTime = 3;
    public bool canBoost = true;

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        boostTime = 3;
	}

	private void FixedUpdate()
	{
        movement = new Vector3(-Input.GetAxis("Vertical"), 0, 0) * speed * (canBoost ? boostMultiplier : 1);
        gameObject.transform.Translate(movement);

        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
	}

	private void Update()
	{
        // Boost handling
        if (Input.GetKey(KeyCode.LeftShift) && boostTime > 0)
            boostTime -= Time.deltaTime;
        else if (boostTime < maxBoostTime)
            boostTime += Time.deltaTime;
        else
            canBoost = true;
        if (boostTime <= 0)
            canBoost = false;
	}
}
