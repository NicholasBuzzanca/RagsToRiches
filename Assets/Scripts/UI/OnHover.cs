using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraMovement.singleton.isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraMovement.singleton.isHovering = false;
    }


}
