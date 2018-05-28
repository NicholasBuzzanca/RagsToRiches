using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public static Inventory singleton;

    public int gold;
    public int inventorySize;

    public GameObject floatingText;
    public GameObject gameUI;

    public Sprite vegImage;
    public Sprite meatImage;
    public Sprite fishImage;
    public Sprite woodImage;
    public Sprite rockImage;
    public Sprite oreImage;
    public Sprite silkImage;
    public Sprite dyeImage;
    public Sprite woolImage;

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
        isOpen = true;

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
                switch (type)
                {
                    case City.Resources.Vegetable:
                        newItem.GetComponent<Image>().sprite = vegImage;
                        break;
                    case City.Resources.Meat:
                        newItem.GetComponent<Image>().sprite = meatImage;
                        break;
                    case City.Resources.Fish:
                        newItem.GetComponent<Image>().sprite = fishImage;
                        break;
                    case City.Resources.Stone:
                        newItem.GetComponent<Image>().sprite = rockImage;
                        break;
                    case City.Resources.Iron:
                        newItem.GetComponent<Image>().sprite = oreImage;
                        break;
                    case City.Resources.Wood:
                        newItem.GetComponent<Image>().sprite = woodImage;
                        break;
                    case City.Resources.Wool:
                        newItem.GetComponent<Image>().sprite = woolImage;
                        break;
                    case City.Resources.Silk:
                        newItem.GetComponent<Image>().sprite = silkImage;
                        break;
                    case City.Resources.Dye:
                        newItem.GetComponent<Image>().sprite = dyeImage;
                        break;
                }
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

    public bool SellToCity(Production prod)
    {
        City city = PlayerMovement.singleton.thisCity;
        if(city == null)
        {
            return false;
        }
        gold += city.Sell(prod.generates, Vector3.Distance(prod.transform.position,city.transform.position));
        return true;
    }

    public void BuyFromCity(City city)
    {
        //if player is not at city return
        if (!ReferenceEquals(PlayerMovement.singleton.thisCity, city))
        {
            SFXManager.singleton.PlayButtonClickSFX();
            return;
        }

        int cost = city.CostToBuy();
        if (cost > gold)
        {
            SFXManager.singleton.PlayButtonClickSFX();
            return;
        }
        //if no inv space return
        if (!HasInventorySpot())
        {
            SFXManager.singleton.PlayButtonClickSFX();
            return;
        }
        if(city.BuyOne())
        {
            gold -= cost;
            //add to inv
            AddItem(city.transform.position,city.export);
            SFXManager.singleton.PlayBuySFX();
        }
        else
        {
            SFXManager.singleton.PlayButtonClickSFX();
            return;
        }
    }

    public bool TakeFromProd(Production prod)
    {
        if (SellToCity(prod))
        {
            prod.resourceAmt--;
            InventoryToolTip.singleton.CloseToolTip();
            return true;
        }
        return false;
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
                    CreateFloatingText("+1 Inventory",house.transform.position);
                }
                if (house.thisBuff == House.Buff.Movespeed)
                {
                    PlayerMovement.singleton.ChangeSpeed(.12f);
                    CreateFloatingText("+1 Movement Speed", house.transform.position);
                }
                house.isOwned = true;
                gold -= (int)house.currPrice;
                SFXManager.singleton.PlayBuySFX();
                return;
            }
        }
        SFXManager.singleton.PlayButtonClickSFX();
    }

    void CreateFloatingText(string text, Vector3 pos)
    {
        GameObject fText = Instantiate(floatingText);
        //?????????????
        fText.transform.GetChild(0).GetComponent<Text>().text = text;

        fText.transform.SetParent(gameUI.transform);

        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);

        fText.transform.position = screenPos;
    }

    public void SellHouse(House house)
    {
        if (!house.isOwned)
        {
            SFXManager.singleton.PlayButtonClickSFX();
            return;
        }
        if (house.thisBuff == House.Buff.Inventory)
        {
            RemoveSlots(1);
            CreateFloatingText("-1 Inventory",house.transform.position);
        }
        if (house.thisBuff == House.Buff.Movespeed)
        {
            PlayerMovement.singleton.ChangeSpeed(-.12f);
            CreateFloatingText("-1 Movement Speed", house.transform.position);
        }
        if (house.isOwned)
        {
            house.isOwned = false;
            house.isForSale = false;
            gold += (int)house.currPrice;
            SFXManager.singleton.PlayBuySFX();
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
                SFXManager.singleton.PlayBuySFX();
                return;
            }
        }
        SFXManager.singleton.PlayButtonClickSFX();
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
