using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankScript : MonoBehaviour
{
    public int life = 400;
    public AudioSource audioSource;
    public bool isPlayer = false;

    public void GetDamages(int damages)
    {
        life -= damages;
        if (life > 0 && life - damages <= 0)
        {
            audioSource.Play();
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
    }
}
