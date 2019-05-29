using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

    public int life = 200;
    public int gunDamages = 10;
    public int missileDamages = 100;
    protected AudioSource audioSource;


    public void GetDamages(int damages)
    {
        life -= damages;
        if (life <= 0)
            audioSource.Play();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
