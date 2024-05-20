using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clickSound;

    public void EasyGame()
    {
        source.PlayOneShot(clickSound);
        SceneManager.LoadScene("Saper");
    }

    public void NormalGame()
    {
        source.PlayOneShot(clickSound);
        SceneManager.LoadScene("Saper 1");
    }

    public void HardGame()
    {
        source.PlayOneShot(clickSound);
        SceneManager.LoadScene("Saper 2");
    }


}
