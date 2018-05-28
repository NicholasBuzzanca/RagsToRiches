using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

    public AudioClip button_hover;
    public AudioClip button_click;

    // Use this for initialization
    void Start () {
		
	}
	
	public void ResetScene()
    {
        GetComponent<AudioSource>().clip = button_click;
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MouseHover()
    {
        GetComponent<AudioSource>().clip = button_hover;
        GetComponent<AudioSource>().Play();
    }
}
