using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DoubleDetector : MonoBehaviour, IPointerClickHandler
{
    private int tap;
    [SerializeField] float interval;
    private bool waitingForDoubleTap;
    public UnityEvent onDoubleTap = new();
    public UnityEvent onSingleTap = new();


    public void OnPointerClick(PointerEventData eventData)
    {
        tap++;
        if (tap == 1)
            StartCoroutine(DoubleTapInterval());
    }

    IEnumerator DoubleTapInterval()
    {
        waitingForDoubleTap = true;
        yield return new WaitForSeconds(interval);
        waitingForDoubleTap = false;

        if (tap >= 2 )
        {
            tap = 0;
            onDoubleTap.Invoke();
        }
        else if (tap == 1 )
        {
            tap = 0;
            onSingleTap.Invoke();
        }
    }

}
