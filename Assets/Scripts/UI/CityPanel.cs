using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityPanel : MonoBehaviour {

    public static CityPanel singleton;


    CanvasGroup cg;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;
        cg = GetComponent<CanvasGroup>();
	}

    public void OpenCityPanel()
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }

    public void CloseCityPanel()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

}
