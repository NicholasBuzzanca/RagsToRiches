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
        SFXManager.singleton.PlayMouseHoverSFX();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryToolTip.singleton.CloseToolTip();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EndGameManager.singleton.isPaused)
            return;
        if (prod == null)
        {
            //try to sell to city
            Inventory.singleton.SellToCity(slotNum);
            InventoryToolTip.singleton.CloseToolTip();
            SFXManager.singleton.PlayBuySFX();
        }
        else
        {
            //put in inventory
            if (Inventory.singleton.TakeFromProd(prod))
                SFXManager.singleton.PlayBuySFX();
            else
                SFXManager.singleton.PlayButtonClickSFX();
        }
    }
}
