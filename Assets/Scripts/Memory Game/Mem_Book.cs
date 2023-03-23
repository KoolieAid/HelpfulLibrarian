using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Asyncoroutine;
using UnityEngine.Events;

public class Mem_Book : MonoBehaviour
{
    // Don't ever edit this
    private enum FlipState
    {
        Cover = 0,
        Back = 180,
    }

    private Camera _camera;
    private bool isFlipping;
    private bool isLocked = false;
    [SerializeField] private FlipState state = FlipState.Cover;
    [SerializeField] private float speed = 10f;
    [SerializeField] [Range(0, 2)] private float rotationPrecision = 1f;

    public UnityEvent<Mem_Book> onTouch;

    public BookInfo info;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    async void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (isLocked) return;
        
        if (isFlipping) return;
        FlipOver();
        await new WaitUntil(() => !isFlipping);
        onTouch.Invoke(this);
    }

    // Only for visual
    public async void FlipOver()
    {
        isFlipping = true;

        var targetState = state == FlipState.Cover ? FlipState.Back : FlipState.Cover;
        var targetRotation = Quaternion.Euler(0, (float)targetState, 0);
        for (;;)
        {
            await new WaitForEndOfFrame();
            state = targetState;

            var currentRotation = transform.localRotation;
            var output = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * speed);
            transform.localRotation = output;

            if (Mathf.Abs((float)targetState - transform.localRotation.eulerAngles.y) <= rotationPrecision)
            {
                break;
            }
        }

        isFlipping = false;
    }

    public void Lock()
    {
        isLocked = true;
    }
}