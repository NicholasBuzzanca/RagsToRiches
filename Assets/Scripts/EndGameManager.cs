using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour {

    public static EndGameManager singleton;

    public GameObject AI;

    public CanvasGroup mainUI;
    public CanvasGroup endUI;
    public CanvasGroup gameUI;
    public CanvasGroup pauseMenu;
    public CanvasGroup howToPlayUI;

    public Text endGameText;

    public Light sun;

    public bool isPaused;
    bool isGameStarted;

    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;

    float sunInitialIntensity;

    public float dayLength;

    float dayTimer;
    float nightTimer;

    float startTime;

	// Use this for initialization
	void Start () {
        singleton = this;
        startTime = Time.time;
        sunInitialIntensity = sun.intensity;
        isPaused = true;
        isGameStarted = false;
        dayTimer = Time.time+.25f;
        nightTimer = Time.time+.25f;
    }
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
            return;
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / dayLength) * timeMultiplier;

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun()
    {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);

        float intensityMultiplier = 1;
        if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f)
        {
            intensityMultiplier = 0;   
        }
        else if (currentTimeOfDay <= 0.25f)
        {
            if (nightTimer < Time.time)
            {
                nightTimer = Time.time + 50f;
                SFXManager.singleton.FadeInDayTime();
            }
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
            if (dayTimer < Time.time)
            {
                dayTimer = Time.time + 50f;
                SFXManager.singleton.FadeOutDayTime();
            }
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }

    int GetElapsedDays()
    {
        return (int)((Time.time - startTime) / dayLength);
    }

    int GetElapsedHours()
    {
        return (int)(currentTimeOfDay * 24);
    }

    int GetElapsedMinutes()
    {
        return (int)(currentTimeOfDay * 1440) - GetElapsedHours() * 60;
    }

    public void OpenEndGame()
    {
        //pause game
        isPaused = true;
        BuildingManager.singleton.EndGame();

        endUI.alpha = 1;
        endUI.blocksRaycasts = true;
        endUI.interactable = true;

        mainUI.alpha = 0;
        mainUI.blocksRaycasts = false;
        mainUI.interactable = false;

        gameUI.alpha = 0;
        gameUI.blocksRaycasts = false;
        gameUI.interactable = false;

        endGameText.text = GetElapsedDays() + " days, " + GetElapsedHours() + " hours, and " + GetElapsedMinutes() + " minutes.";
    }

    public void OpenMainMenu()
    {
        SFXManager.singleton.PlayButtonClickSFX();

        isPaused = true;
        AI.SetActive(false);
        mainUI.alpha = 1;
        mainUI.blocksRaycasts = true;
        mainUI.interactable = true;

        endUI.alpha = 0;
        endUI.blocksRaycasts = false;
        endUI.interactable = false;

        gameUI.alpha = 0;
        gameUI.blocksRaycasts = false;
        gameUI.interactable = false;

        howToPlayUI.alpha = 0;
        howToPlayUI.blocksRaycasts = false;
        howToPlayUI.interactable = false;
    }

    public void OpenGameUI()
    {
        isPaused = false;
        isGameStarted = true;
        AI.SetActive(true);
        gameUI.alpha = 1;
        gameUI.blocksRaycasts = true;
        gameUI.interactable = true;

        mainUI.alpha = 0;
        mainUI.blocksRaycasts = false;
        mainUI.interactable = false;

        endUI.alpha = 0;
        endUI.blocksRaycasts = false;
        endUI.interactable = false;
    }

    public void OpenHowToPlay()
    {
        SFXManager.singleton.PlayButtonClickSFX();

        howToPlayUI.alpha = 1;
        howToPlayUI.blocksRaycasts = true;
        howToPlayUI.interactable = true;

        mainUI.alpha = 0;
        mainUI.blocksRaycasts = false;
        mainUI.interactable = false;

        gameUI.alpha = 0;
        gameUI.blocksRaycasts = false;
        gameUI.interactable = false;

        endUI.alpha = 0;
        endUI.blocksRaycasts = false;
        endUI.interactable = false;
    }

    public void TogglePause()
    {
        if (!isGameStarted)
            return;

        if(isPaused)
        {
            isPaused = false;
            UnpauseGame();
        }
        else
        {
            isPaused = true;
            PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenu.alpha = 1;
        pauseMenu.interactable = true;
        pauseMenu.blocksRaycasts = true;
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
        Time.timeScale = 1;
    }
}
