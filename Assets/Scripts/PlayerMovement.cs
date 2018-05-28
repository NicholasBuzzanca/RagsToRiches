﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement singleton;

    NavMeshAgent navMeshAgent;

    public City thisCity;
    public Production thisProd;
    bool isMovingToCity;
    Location movingTo;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndGameManager.singleton.TogglePause();
        }
        if (EndGameManager.singleton.isPaused)
            return;
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
                    ProductionPanel.singleton.CloseProdPanel();
                    SFXManager.singleton.PlayButtonClickSFX();
                    return;
                }
                if(!CameraMovement.singleton.isHovering && CityPanel.singleton.isOpen())
                {
                    CityPanel.singleton.CloseCityPanel();
                    SFXManager.singleton.PlayButtonClickSFX();
                }
                House house = hit.transform.GetComponent<House>();
                if(house != null)
                {
                    //buy house or sell house
                    if(house.isOwned)
                    {
                        Inventory.singleton.SellHouse(house);
                    }
                    else if(house.isForSale)
                    {
                        Inventory.singleton.BuyHouse(house);
                    }
                }
                Production prod = hit.transform.GetComponent<Production>();
                if(prod != null)
                {
                    //buy prod or open prodpanel
                    if(prod.isOwned)
                    {
                        ProductionPanel.singleton.OpenProdPanel(prod);
                        SFXManager.singleton.PlayButtonClickSFX();
                    }
                    else if(prod.isForSale)
                    {
                        Inventory.singleton.BuyProd(prod);
                    }
                    CityPanel.singleton.CloseCityPanel();
                }
                else if (!CameraMovement.singleton.isHovering)
                {
                    ProductionPanel.singleton.CloseProdPanel();
                    SFXManager.singleton.PlayButtonClickSFX();
                }
                Mansion mansion = hit.transform.GetComponent<Mansion>();
                if(mansion != null)
                {
                    mansion.BuyMansion();
                    SFXManager.singleton.PlayButtonClickSFX();
                }
            }
            else
            {
                CityPanel.singleton.CloseCityPanel();
                ProductionPanel.singleton.CloseProdPanel();
            }
        }
        //handle city location
        //if (!ReferenceEquals(movingTo, thisCity))
        //{
        if (navMeshAgent.remainingDistance - navMeshAgent.stoppingDistance < 2f)
        {
            if (isMovingToCity)
                thisCity = (City)movingTo;
            else
                thisProd = (Production)movingTo;
        }
        else
        {
            thisCity = null;
            thisProd = null;

        }
        //}
    }

    public void ChangeSpeed(float amt)
    {
        navMeshAgent.speed += amt;
    }

    public void MoveToCity(City city)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(city.transform.position);
        movingTo = city;
        thisCity = null;
        thisProd = null;
        isMovingToCity = true;
    }

    public void MoveToProduction(Production prod)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(prod.transform.position);
        movingTo = prod;
        thisCity = null;
        thisProd = null;
        isMovingToCity = false;
    }

}
