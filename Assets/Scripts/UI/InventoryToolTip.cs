using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToolTip : MonoBehaviour {

    public static InventoryToolTip singleton;

    CanvasGroup cg;
    public bool isOpen;
    public Text text;
    City.Resources type;
    Vector3 origin;


    // Use this for initialization
    void Start () {
        if (singleton == null)
            singleton = this;
        cg = GetComponent<CanvasGroup>();
        isOpen = false;
	}

    void LateUpdate()
    {
        if(isOpen)
        {
            text.text = "Sell Price: ";
            if (PlayerMovement.singleton.thisCity == null)
            {
                text.text += "N/A";
            }
            else
            {
                text.text += PlayerMovement.singleton.thisCity.GetPrice(type, Vector3.Distance(PlayerMovement.singleton.thisCity.transform.position, origin));
            }
        }
    }

    public void OpenToolTip(Vector2 pos, City.Resources type, Vector3 origin)
    {
        isOpen = true;

        this.type = type;
        this.origin = origin;

        transform.position = pos;
        
        cg.alpha = 1;
    }

    public void CloseToolTip()
    {
        isOpen = false;
        cg.alpha = 0;
    }
}
