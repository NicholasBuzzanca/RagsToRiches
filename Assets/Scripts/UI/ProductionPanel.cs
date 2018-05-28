using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour {

    public static ProductionPanel singleton;

    public Sprite oreImage;
    public Sprite meatImage;
    public Sprite dyeImage;

    public Text type;
    public GameObject item;
    public Transform slotHolder;

    List<GameObject> slots;
    CanvasGroup cg;

    Production thisProd;
    City.Resources thisType;
    int numGenerated;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        numGenerated = 0;
        thisType = City.Resources.None;
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

        if(thisProd.resourceAmt == numGenerated && thisProd.generates == thisType)
        {
            return;
        }

        thisType = thisProd.generates;
        numGenerated = thisProd.resourceAmt;
        InventoryToolTip.singleton.CloseToolTip();

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
                switch (thisProd.generates)
                {
                    case City.Resources.Meat:
                        newItem.GetComponent<Image>().sprite = meatImage;
                        break;
                    case City.Resources.Iron:
                        newItem.GetComponent<Image>().sprite = oreImage;
                        break;
                    case City.Resources.Dye:
                        newItem.GetComponent<Image>().sprite = dyeImage;
                        break;
                }
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
        if (EndGameManager.singleton.isPaused)
            return;
        Inventory.singleton.SellProd(thisProd);
        SFXManager.singleton.PlayBuySFX();
        CloseProdPanel();
    }

    public void MovePlayerHere()
    {
        if (EndGameManager.singleton.isPaused)
            return;
        PlayerMovement.singleton.MoveToProduction(thisProd);
    }
}
