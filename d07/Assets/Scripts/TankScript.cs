using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankScript : MonoBehaviour
{
    public int life;
    public int maxLife = 400;
    private AudioSource audioSource;
    public bool isPlayer = false;
    public GameObject explosion;
    private CameraScript cameraScript;

    public void GetDamages(int damages)
    {
        life -= damages;
        if (life > 0 && life - damages <= 0)
        {
            if (isPlayer && life < maxLife / 2)
                cameraScript.PanicMusic();
            audioSource.Play();
            explosion.SetActive(true);
            if (!isPlayer)
                Destroy(gameObject, 2f);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Use this for initialization
    protected void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        cameraScript = Camera.main.GetComponent<CameraScript>();
        life = maxLife;
    }
}
