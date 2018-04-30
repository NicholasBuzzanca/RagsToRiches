using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler {

    public City.Resources type;
    public Vector3 location;
    public int slotNum;
    public Production prod;

    public void Init(City.Resources type, Vector3 location, int slotNum, Production prod)
    {
        this.type = type;
        this.location = location;
        this.slotNum = slotNum;
        this.prod = prod;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryToolTip.singleton.OpenToolTip(eventData.position, type, location);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryToolTip.singleton.CloseToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (prod == null)
        {
            //try to sell to city
            Inventory.singleton.SellToCity(slotNum);
            InventoryToolTip.singleton.CloseToolTip();
        }
        else
        {
            //put in inventory
            Inventory.singleton.TakeFromProd(prod);
        }
    }
}
