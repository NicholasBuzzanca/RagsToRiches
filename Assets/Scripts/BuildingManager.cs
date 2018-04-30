using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager singleton;

    List<House> houses;
    List<Production> productions;

    float timer;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        houses = new List<House>();
        productions = new List<Production>();
    }
	
	// Update is called once per frame
	void Update () {
		if(timer < Time.time)
        {
            timer = Time.time + 5f;
            if (Random.Range(0, 1) > .7f)
            {
                if (!SellHouse())
                    SellProduction();
            }
            else
            {
                if (!SellProduction())
                    SellHouse();
            }
        }
	}

    public bool SellHouse()
    {
        for (int i = 0; i < houses.Count; i++)
        {
            if(!houses[i].isForSale)
            {
                if(houses[i].PutUpForSale(Inventory.singleton.PlayerValue))
                    return true;
            }
        }
        return false;
    }

    public bool SellProduction()
    {
        for (int i = 0; i < productions.Count; i++)
        {
            if (!productions[i].isForSale)
            {
                if (productions[i].PutUpForSale(Inventory.singleton.PlayerValue))
                    return true;
            }
        }
        return false;
    }

    public void CheckInHouse(House house)
    {
        houses.Add(house);
    }

    public void CheckInProduction(Production prod)
    {
        productions.Add(prod);
    }


}
