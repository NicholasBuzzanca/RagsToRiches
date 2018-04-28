using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler, IPointerExitHandler {

    public City.Resources type;
    public Vector3 location;
    public int slotNum;

    public void Init(City.Resources type, Vector3 location, int slotNum)
    {
        this.type = type;
        this.location = location;
        this.slotNum = slotNum;
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
        Inventory.singleton.SellToCity(slotNum);
        InventoryToolTip.singleton.CloseToolTip();
    }
}
