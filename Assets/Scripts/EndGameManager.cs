using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour {

    public static EndGameManager singleton;

    public CanvasGroup mainUI;
    public CanvasGroup endUI;
    public CanvasGroup gameUI;

    public Text endGameText;

    public Light sun;

    bool isPaused;

    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    [HideInInspector]
    public float timeMultiplier = 1f;

    float sunInitialIntensity;

    public float dayLength;

    float startTime;

	// Use this for initialization
	void Start () {
        singleton = this;
        startTime = Time.time;
        sunInitialIntensity = sun.intensity;
        isPaused = true;
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
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.73f)
        {
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
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
        isPaused = true;
        mainUI.alpha = 1;
        mainUI.blocksRaycasts = true;
        mainUI.interactable = true;

        endUI.alpha = 0;
        endUI.blocksRaycasts = false;
        endUI.interactable = false;

        gameUI.alpha = 0;
        gameUI.blocksRaycasts = false;
        gameUI.interactable = false;
    }

    public void OpenGameUI()
    {
        isPaused = false;
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
}
