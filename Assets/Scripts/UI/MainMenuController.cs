using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void StartGame()
    {
        EndGameManager.singleton.OpenGameUI();
        BuildingManager.singleton.StartGame();
    }

    public void OpenSettings()
    {
        
    }

    public void OpenHowToPlay()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
