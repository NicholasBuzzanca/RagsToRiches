using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement singleton;

    NavMeshAgent navMeshAgent;

    public City thisCity;
    City movingTo;
    public Camera cam;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        cam = Camera.main;
        thisCity = null;
        navMeshAgent = GetComponent<NavMeshAgent>();
	}

    void Update()
    {
        //handle city selection
        if (Input.GetMouseButtonDown(0) && !CameraMovement.singleton.isHovering)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                City city = hit.transform.GetComponent<City>();
                if (city != null)
                {
                    CityPanel.singleton.OpenCityPanel(city);
                }
                else if(!CameraMovement.singleton.isHovering)
                {
                    CityPanel.singleton.CloseCityPanel();
                }
            }
        }
        //handle city location
        //if (!ReferenceEquals(movingTo, thisCity))
        //{
        if (navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance < .05f)
        {
            thisCity = movingTo;
        }
        else
        {
            thisCity = null;
        }
        //}
    }

    public void MoveToCity(City city)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(city.transform.position);
        movingTo = city;
        thisCity = null;
    }

}
