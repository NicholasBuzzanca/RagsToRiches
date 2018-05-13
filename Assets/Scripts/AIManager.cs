using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {

    public static AIManager singleton;

    public GameObject[] cities;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;
	}
	
	public City GetDestination()
    {
        int index = Random.Range(0,9);
        return cities[index].GetComponent<City>();
    }
}
