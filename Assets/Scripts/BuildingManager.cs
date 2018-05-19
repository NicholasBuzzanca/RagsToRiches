using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager singleton;

    bool inGame;

    List<House> houses;
    List<Production> productions;

    float timer;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        inGame = false;
        houses = new List<House>();
        productions = new List<Production>();
    }
	
	// Update is called once per frame
	void Update () {
		if(timer < Time.time && inGame)
        {
            timer = Time.time + 2f;
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

    public void StartGame()
    {
        inGame = true;
        Mansion.singleton.cg.alpha = 1;
    }

    public void EndGame()
    {
        Mansion.singleton.cg.alpha = 0;

        foreach(House h in houses)
        {
            h.cg.alpha = 0;
        }
        foreach(Production p in productions)
        {
            p.cg.alpha = 0;
        }
        inGame = false;
    }

    //starts at a random spot in the houses array
    public bool SellHouse()
    {
        int i = Random.Range(0,houses.Count);
        int count = 0;
        while (count < 2)
        {
            for (int j = i; j < houses.Count; j++)
            {
                if (!houses[j].isForSale)
                {
                    if (houses[j].PutUpForSale(Inventory.singleton.PlayerValue))
                        return true;
                }
            }
            count++;
            i = 0;
        }
        return false;
    }

    public bool SellProduction()
    {
        int j = Random.Range(0, houses.Count);
        int count = 0;
        while (count < 2)
        {
            for (int i = j; i < productions.Count; i++)
            {
                if (!productions[i].isForSale)
                {
                    if (productions[i].PutUpForSale(Inventory.singleton.PlayerValue))
                        return true;
                }
            }
            count++;
            j = 0;
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
