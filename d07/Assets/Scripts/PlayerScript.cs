using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : TankScript
{
    private Vector3 movement;
    public float speed = 0.1f;
    public float rotationSpeed = 1;
    public float boostMultiplier = 2;
    public float boostTime;
    public float maxBoostTime = 3;
    public bool canBoost = true;
    public Text TextLife;
    public Text missileText;
    public Text boostText;
    public CanonScript canon;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        boostTime = 3;
    }

    private void FixedUpdate()
    {
        movement = new Vector3(-Input.GetAxis("Vertical"), 0, 0) * speed 
            * (canBoost && Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1);
        gameObject.transform.Translate(movement);

        transform.Rotate(0, Input.GetAxis("Horizontal"), 0);
    }

    private void Update()
    {
        // Boost handling
        if (canBoost && Input.GetKey(KeyCode.LeftShift) && boostTime > 0)
            boostTime -= Time.deltaTime;
        else if (boostTime < maxBoostTime)
            boostTime += Time.deltaTime;
        else
            canBoost = true;
        if (boostTime <= 0)
            canBoost = false;

        TextLife.text = life + "/\n" + maxLife;
        missileText.text = "" + canon.missiles;
        boostText.text = "" + (boostTime >= maxBoostTime ? maxBoostTime : Mathf.Round(boostTime)) + "/" + maxBoostTime;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
