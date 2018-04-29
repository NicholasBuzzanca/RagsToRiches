using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager singleton;

    List<House> houses;

    float timer;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        houses = new List<House>();
	}
	
	// Update is called once per frame
	void Update () {
		if(timer < Time.time)
        {
            timer = Time.time + 5f;
            SellHouse();
        }
	}

    public void SellHouse()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            if(!houses[i].isForSale)
            {
                if(houses[i].PutUpForSale(Inventory.singleton.PlayerValue))
                    return;
            }
        }
    }

    public void CheckInHouse(House house)
    {
        houses.Add(house);
    }

    
}
