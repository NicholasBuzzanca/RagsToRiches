using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityPanel : MonoBehaviour {

    public static CityPanel singleton;

    public Text nametext;

    public Text exportText;
    public Text foodtext;
    public Text materialtext;
    public Text luxurytext;

    public Text buyCost;

    City thisCity;

    CanvasGroup cg;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;
        cg = GetComponent<CanvasGroup>();
	}

    void Update()
    {
        if(thisCity != null)
        {
            thisCity.UpdateStock();
            UpdateCityDisplay();
        }
    }

    void UpdateCityDisplay()
    {
        nametext.text = thisCity.cityName;

        exportText.text = "Exports: " + thisCity.export;
        buyCost.text = thisCity.CostToBuy() + "";

        foodtext.text = thisCity.foodStock + "";
        materialtext.text = thisCity.materialStock + "";
        luxurytext.text = thisCity.luxuryStock + "";
    }

    public bool isOpen()
    {
        if (cg.alpha == 0)
            return false;
        return true;
    }

    public void OpenCityPanel(City city)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        thisCity = city;

        //enter data
        UpdateCityDisplay();
    }

    public void CloseCityPanel()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        thisCity = null;
    }

    public void MovePlayerHere()
    {
        if (EndGameManager.singleton.isPaused)
            return;
        SFXManager.singleton.PlayButtonClickSFX();
        PlayerMovement.singleton.MoveToCity(thisCity);
    }

    public void Buy()
    {
        if (EndGameManager.singleton.isPaused)
            return;
        Inventory.singleton.BuyFromCity(thisCity);
    }
}
