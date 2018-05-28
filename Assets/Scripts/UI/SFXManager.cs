using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

    public static SFXManager singleton;

    public GameObject fires;

    public AudioClip button_hover;
    public AudioClip button_click;
    public AudioClip buy_sfx;

    public AudioClip daytime_sfx;
    public AudioClip nighttime_sfx;
    public AudioClip water_sfx;

    AudioSource hover;
    AudioSource click;
    AudioSource buy;
    AudioSource daytime;
    AudioSource nighttime;
    AudioSource water;

    // Use this for initialization
    void Start () {
        singleton = this;
        hover = gameObject.AddComponent<AudioSource>();
        click = gameObject.AddComponent<AudioSource>();
        buy = gameObject.AddComponent<AudioSource>();
        daytime = gameObject.AddComponent<AudioSource>();
        nighttime = gameObject.AddComponent<AudioSource>();
        water = gameObject.AddComponent<AudioSource>();

        water.clip = water_sfx;
        hover.clip = button_hover;
        click.clip = button_click;
        buy.clip = buy_sfx;
        daytime.clip = daytime_sfx;
        nighttime.clip = nighttime_sfx;

        buy.volume = .55f;
        daytime.loop = true;
        nighttime.loop = true;
        daytime.volume = 0f;
        nighttime.volume = 0f;

        daytime.Play();
        nighttime.Play();
        water.volume = 0;
        water.loop = true;
        water.Play(); 
    }
    public void StartWaterSFX()
    {
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        water.volume = .3f;
    }

    public void FadeOutDayTime()
    {
        fires.SetActive(true);
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        StartCoroutine(fadeVol(daytime,-.025f));
        StartCoroutine(fadeVol(nighttime, .025f));
    }

    IEnumerator fadeVol(AudioSource src, float amt)
    {
        int i = 0;
        while (i < 20)
        {
            yield return new WaitForSeconds(.05f);
            src.volume += amt;
            i++;
        }
    }

    public void FadeInDayTime()
    {
        fires.SetActive(false);
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        StartCoroutine(fadeVol(daytime, .025f));
        StartCoroutine(fadeVol(nighttime, -.025f));
    }

    public void PlayBuySFX()
    {
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        buy.Play();
    }

    public void PlayButtonClickSFX()
    {
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        click.Play();
    }

    public void PlayMouseHoverSFX()
    {
        if (PersistentData.singleton.isMuted)
        {
            return;
        }
        hover.Play();
    }
}
