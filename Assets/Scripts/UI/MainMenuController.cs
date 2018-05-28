using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour {

    public AudioClip button_hover;
    public AudioClip button_click;
  
    // Use this for initialization
    void Start () {
		if(PersistentData.singleton == null)
        {
            GameObject go = new GameObject();
            go.AddComponent<PersistentData>();  
        }
	}
	
	public void StartGame()
    {
        if (!PersistentData.singleton.isMuted)
        {
            GetComponent<AudioSource>().clip = button_click;
            GetComponent<AudioSource>().Play();
            SFXManager.singleton.StartWaterSFX();
        }

        EndGameManager.singleton.OpenGameUI();
        BuildingManager.singleton.StartGame();
    }

    public void OpenSettings()
    {
        //AudioSource.PlayClipAtPoint(button_click,Vector3.zero);
        if (PersistentData.singleton.isMuted)
            return;
        GetComponent<AudioSource>().clip = button_click;
        GetComponent<AudioSource>().Play();
    }

    public void OpenHowToPlay()
    {
        if (PersistentData.singleton.isMuted)
            return;
        GetComponent<AudioSource>().clip = button_click;
        GetComponent<AudioSource>().Play();
    }

    public void QuitGame()
    {
        if (!PersistentData.singleton.isMuted)
        {
            GetComponent<AudioSource>().clip = button_click;
            GetComponent<AudioSource>().Play();
        }
        Application.Quit();
    }

    public void MouseHover()
    {
        if (PersistentData.singleton.isMuted)
            return;
        GetComponent<AudioSource>().clip = button_hover;
        GetComponent<AudioSource>().Play();
    }

    public void ChangeMuteStatus()
    {
        GetComponent<AudioSource>().clip = button_click;
        GetComponent<AudioSource>().Play();
        PersistentData.singleton.isMuted = !PersistentData.singleton.isMuted;
    }
}
