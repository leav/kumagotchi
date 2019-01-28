using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{
    public UnityEngine.Events.UnityEvent handler = new UnityEngine.Events.UnityEvent();

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        handler.Invoke();
    }
}
