using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    /// <summary>
    /// Caution, this will run in every frame if the object is <b>being</b> dragged
    /// </summary>
    ///
    /// <remarks>
    /// This is not exposed to the editor so aint nobody can touch it
    /// </remarks>
    public System.Action<PointerEventData> onDrag;

    public UnityEvent onBeginDrag;
    
    public UnityEvent onEndDrag;

    public void OnDrag(PointerEventData eventData)
    {
        onDrag.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag.Invoke();
    }
}
