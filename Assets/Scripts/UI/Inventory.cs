using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory singleton;

    CanvasGroup cg;
    bool isOpen;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        cg = GetComponent<CanvasGroup>();
        isOpen = false;
	}

    public void ToggleInventory()
    {
        if(isOpen)
        {
            cg.alpha = 0;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
        else
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
        isOpen = !isOpen;;
    }
}
