﻿using System.Collections;
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
        for (int i = 0; (i < amt || slots.Count != 16); i++)
        {
            GameObject s = Instantiate(slot);
            slots.Add(s);
            s.transform.SetParent(slotPanel.transform);
            slotCount++;
        }
    }

    //todo handle items in removed slots
    public void RemoveSlots(int amt)
    {
        for (int i = slots.Count-1; i > slots.Count-1-amt; i--)
        {
            if (slotCount <= 16)
            {
                slots.RemoveAt(i);
                Destroy(slotPanel.transform.GetChild(i));
            }
            slotCount--;
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
                newItem.GetComponent<Item>().Init(type, origin, i);

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
}
