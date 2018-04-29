using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : MonoBehaviour {

    public bool isForSale;
    public bool isOwned;

    float cdTimer;
    float endTimer;

    public float baseValue;
    public float currPrice;
    float deltaValue;

    float startTime;

    public Text text;

    public CanvasGroup cg;

    //change so certain values and randomized ex: when it switches to sin value
    void Start () {
        //isForSale = false;
        //isOwned = false;
        BuildingManager.singleton.CheckInHouse(this);
        deltaValue = 0;
        currPrice = baseValue;
        cdTimer = 0;
	}

    public bool PutUpForSale(float value)
    {
        if (cdTimer > Time.time)
            return false;
        isForSale = true;
        baseValue *= value;
        startTime = Time.time;
        currPrice = Val(Time.time, baseValue);
        endTimer = Time.time + Random.Range(8f,18f);
        cdTimer = Time.time + 10;
        return true;
    }

    void FixedUpdate()
    {
        if(isOwned || isForSale)
        {  
            if(endTimer < Time.time && !isOwned)
            {
                isForSale = false;
                cdTimer = Time.time + 10;
            }
            deltaValue = Val(startTime, baseValue) - currPrice;
            
            currPrice += deltaValue;
        }

        DisplayPrice();
    }

    void DisplayPrice()
    {
        if(isOwned)
        {
            cg.alpha = 1;
            if(deltaValue > 0)
                text.color = Color.green;
            else
                text.color = Color.red;
        }
        else if(isForSale)
        {
            cg.alpha = 1;
            text.color = Color.black;
        }
        else
        {
            cg.alpha = 0;
        }
        text.text = "$" + ((int)currPrice);
    }

    static float Val(float startTime, float baseValue)
    {
        float priceMulti = 0;
        if (Time.time - startTime > 12)
        {
            //sin wave
            priceMulti = ((Mathf.Sin(.5f * (Time.time - startTime) - 12) / 12) + .5f);
        }
        else
        { 
            //upside down parabola
            priceMulti = (-(1f / (100)) * Mathf.Pow(Time.time - startTime - 5, 2f) + 1);
        }
        //priceMulti += RandomFromDistribution.RandomRangeNormalDistribution(-.1f, .1f, RandomFromDistribution.ConfidenceLevel_e._60);

        return baseValue * priceMulti;
    }
}
