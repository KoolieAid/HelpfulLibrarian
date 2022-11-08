using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DoubleDetector : MonoBehaviour, IPointerClickHandler
{
    private int tap;
    [SerializeField] float interval = 5f;
    private bool readyForDoubleTap;
    public UnityEvent onDoubleTap = new();
    public UnityEvent onSingleTap = new();


    public void OnPointerClick(PointerEventData eventData)
    {
        tap ++;
 
        if (tap ==1)
        {
            // onSingleTap.Invoke();
            StartCoroutine(DoubleTapInterval() );
        }
 
        else if (tap>1 && readyForDoubleTap)
        {
            onSingleTap.Invoke();
            tap = 0;
            readyForDoubleTap = false;
        }
        else if (tap > 1 && !readyForDoubleTap)
        {
            onDoubleTap.Invoke();
        }
    }
 
    IEnumerator DoubleTapInterval()
    {  
        yield return new WaitForSeconds(interval);
        readyForDoubleTap = true;
    }
}
