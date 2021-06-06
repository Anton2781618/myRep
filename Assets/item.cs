using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class item : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler,IDropHandler
{

    public CanvasGroup canvasGroup;
    public void OnBeginDrag(PointerEventData eventData)
    {
      //  Debug.Log("OnBeginDrag "+"начали тащить");
       canvasGroup.blocksRaycasts=false;
    }

    public void OnDrag(PointerEventData eventData)
    {
       transform.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
      // Debug.Log("OnDrag "+"Тащим");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop "+"бросили "+transform.name+" "+eventData.pointerDrag);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Debug.Log("OnEndDrag "+"перестали тащить");
       canvasGroup.blocksRaycasts=true;
    }
}
