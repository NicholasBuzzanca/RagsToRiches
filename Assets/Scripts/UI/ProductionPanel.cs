using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour {

    public static ProductionPanel singleton;

    public Text type;
    public GameObject item;
    public Transform slotHolder;

    List<GameObject> slots;
    CanvasGroup cg;

    Production thisProd;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        cg = GetComponent<CanvasGroup>();
        slots = new List<GameObject>();
        for (int i = 0; i < 9; i++)
        {
            slots.Add(slotHolder.GetChild(i).gameObject);
        }
	}

    void Update()
    {
        if (thisProd != null)
        {
            UpdateProdDisplay();
        }
    }

    public void OpenProdPanel(Production prod)
    {
        cg.alpha = 1;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        thisProd = prod;

        //enter data
        UpdateProdDisplay();
    }

    void UpdateProdDisplay()
    {
        type.text = thisProd.generates + "";

        for (int i = 0; i < 9; i++)
        {
            if (slots[i].transform.childCount > 0)
                Destroy(slots[i].transform.GetChild(0).gameObject);
            if(i < thisProd.resourceAmt)
            {
                //add resource here
                GameObject newItem = Instantiate(item);
                newItem.GetComponent<Item>().Init(thisProd.generates, thisProd.transform.position, i, thisProd);

                Vector3 scale = newItem.transform.localScale;
                Quaternion rot = newItem.transform.localRotation;

                newItem.transform.SetParent(slots[i].transform);

                newItem.transform.localScale = Vector3.one;
                newItem.transform.localRotation = rot;
                newItem.transform.localPosition = Vector3.zero;

                //2d image code here

                return;
            }
        }
    }

    public void CloseProdPanel()
    {
        cg.alpha = 0;
        cg.blocksRaycasts = false;
        cg.interactable = false;
        thisProd = null;
    }

    public void SellProduction()
    {
        Inventory.singleton.SellProd(thisProd);
        CloseProdPanel();
    }

    public void MovePlayerHere()
    {
        PlayerMovement.singleton.MoveToProduction(thisProd);
    }
}
