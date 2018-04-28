using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {

    public bool isForSale;
    public bool isOwned;

    public float baseValue;
    public float currPrice;
    public float deltaValue;

    public float startTime;
    float timer;

    float accel;
    float maxDelta;

    //overall price should trend down, but improve when first entering market
    // Use this for initialization
    void Start () {
        //isForSale = false;
        //isOwned = false;
        BuildingManager.singleton.CheckInHouse(this);
        deltaValue = 0;
        currPrice = baseValue;

        accel = -.01f;
        timer = Time.time + 4f;
        maxDelta = .5f;
	}

    void FixedUpdate()
    {
        if(isOwned || isForSale)
        {
            //calculate and display price
            if (Time.time > timer)
            {
                timer = Time.time + 8f;
                accel = -accel;
            }
            deltaValue += accel*Time.fixedDeltaTime;
            deltaValue = Mathf.Clamp(deltaValue, -maxDelta, maxDelta);
            //deltaValue = Val(startTime);// *Time.fixedDeltaTime*baseValue;
            deltaValue += RandomFromDistribution.RandomRangeNormalDistribution(-.1f, .1f, RandomFromDistribution.ConfidenceLevel_e._60)*Time.fixedDeltaTime;
            currPrice += deltaValue;
        }
    }

    static float Val(float startTime)
    {
        return Mathf.Sin((Time.time - startTime)/2f);
    }
}
