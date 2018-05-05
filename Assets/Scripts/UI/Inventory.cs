using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public static Inventory singleton;

    public int gold;
    public int inventorySize;

    public GameObject slot;
    public GameObject item;
    public GameObject slotPanel;
    public Text goldText;

    List<GameObject> slots;
    int slotCount;

    public float PlayerValue;

    CanvasGroup cg;
    bool isOpen;

	// Use this for initialization
	void Start () {
        if (singleton == null)
            singleton = this;

        gold = 100;
        cg = GetComponent<CanvasGroup>();
        inventorySize = 3;
        isOpen = false;

        slots = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            GameObject s = Instantiate(slot);
            slots.Add(s);
            s.transform.SetParent(slotPanel.transform);
        }
        slotCount = 3;
        PlayerValue = 1;
	}

    void Update()
    {
        if(isOpen)
        {
            goldText.text = gold + "";
        }
    }

    public void AddSlots(int amt)
    {
        slotCount += amt;
        for (int i = 0; (i < amt && slots.Count != 16); i++)
        {
            GameObject s = Instantiate(slot);
            slots.Add(s);
            s.transform.SetParent(slotPanel.transform);
            
        }
    }

    //todo handle items in removed slots
    public void RemoveSlots(int amt)
    {
        slotCount -= amt;
        if (slotCount >= 16)
            return;

        for (int i = slots.Count; i > slotCount; i--)
        {
            Destroy(slots[i - 1]);
            slots.RemoveAt(i-1);            
        }
    }

    public void AddItem(Vector3 origin, City.Resources type)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].transform.childCount == 0)
            {
                //add item here
                GameObject newItem = Instantiate(item);
                newItem.GetComponent<Item>().Init(type, origin, i, null);

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

    public void RemoveItem(int slot)
    {
        Destroy(slots[slot].transform.GetChild(0).gameObject);
    }

    public bool HasInventorySpot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].transform.childCount == 0)
            {
                return true;
            }
        }
        return false;
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

    public void SellToCity(int slot)
    {

        City city = PlayerMovement.singleton.thisCity;
        if (city == null)
            return;
        if (!ReferenceEquals(PlayerMovement.singleton.thisCity, city))
            return;

        Item item = slots[slot].transform.GetChild(0).GetComponent<Item>();
        gold += city.Sell(item.type,Vector3.Distance(item.location, city.transform.position));
        RemoveItem(slot);
    }

    public void BuyFromCity(City city)
    {
        //if player is not at city return
        if (!ReferenceEquals(PlayerMovement.singleton.thisCity,city))
            return;

        int cost = city.CostToBuy();
        if (cost > gold)
            return;
        //if no inv space return
        if (!HasInventorySpot())
            return;
        if(city.BuyOne())
        {
            gold -= cost;
            //add to inv
            AddItem(city.transform.position,city.export);
        }
        else
        {
            return;
        }
    }

    public void TakeFromProd(Production prod)
    {
        if (!HasInventorySpot())
            return;
        if (!ReferenceEquals(PlayerMovement.singleton.thisProd, prod))
            return;
        prod.resourceAmt--;
        AddItem(prod.transform.position, prod.generates);
    }

    public void BuyHouse(House house)
    {
        if (house.isForSale && !house.isOwned)
        {
            if(house.currPrice < gold)
            {
                if(house.thisBuff == House.Buff.Inventory)
                {
                    AddSlots(1);
                }
                if (house.thisBuff == House.Buff.Movespeed)
                {
                    PlayerMovement.singleton.ChangeSpeed(.12f);
                }
                house.isOwned = true;
                gold -= (int)house.currPrice;
            }
        }
    }

    public void SellHouse(House house)
    {
        if (house.thisBuff == House.Buff.Inventory)
        {
            RemoveSlots(1);
        }
        if (house.thisBuff == House.Buff.Movespeed)
        {
            PlayerMovement.singleton.ChangeSpeed(-.12f);
        }
        if (house.isOwned)
        {
            house.isOwned = false;
            house.isForSale = false;
            gold += (int)house.currPrice;
        }
    }

    public void BuyProd(Production prod)
    {
        if(prod.isForSale && !prod.isOwned)
        {
            if(prod.currPrice < gold)
            {
                prod.isOwned = true;
                prod.resourceAmt = 0;
                gold -= (int)prod.currPrice;
                prod.genTimer = Time.time + 15f;
            }
        }
    }

    public void SellProd(Production prod)
    {
        if(prod.isOwned)
        {
            prod.isOwned = false;
            prod.isForSale = false;
            gold += (int)prod.currPrice;
        }
    }
}
