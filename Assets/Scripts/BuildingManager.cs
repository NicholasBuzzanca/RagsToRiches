using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager singleton;

    List<House> houses;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        houses = new List<House>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckInHouse(House house)
    {
        houses.Add(house);
    }

    
}
