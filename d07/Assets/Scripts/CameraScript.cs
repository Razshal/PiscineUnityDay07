using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public AudioSource audioSource;
    private bool panic = false;
    public AudioClip panicMusic;

	// Use this for initialization
	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	}
	
    public void PanicMusic()
    {
        if (!panic)
        {
            audioSource.Stop();
            audioSource.clip = panicMusic;
            audioSource.Play();
			panic = true;
        }
    }
}
