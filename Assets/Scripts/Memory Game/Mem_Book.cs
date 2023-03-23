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
    [SerializeField] private FlipState state = FlipState.Cover;
    [SerializeField] private float speed = 10f;
    [SerializeField] [Range(0, 1)] private float rotationPrecision = 1f;

    public UnityEvent<Mem_Book> onTouch;

    [SerializeField] private BookInfo info;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    async void OnMouseOver()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        FlipOver();
        await new WaitUntil(() => !isFlipping);
        onTouch.Invoke(this);
    }

    // Only for visual
    public async void FlipOver()
    {
        if (isFlipping) return;
        isFlipping = true;

        var targetState = state == FlipState.Cover ? FlipState.Back : FlipState.Cover;
        var targetRotation = Quaternion.Euler(0, (float)targetState, 0);
        for (;;)
        {
            await new WaitForEndOfFrame();
            state = targetState;

            var currentRotation = transform.localRotation;
            var output = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * 10f);
            transform.localRotation = output;

            if (Mathf.Abs((float)targetState - transform.localRotation.eulerAngles.y) < rotationPrecision)
            {
                break;
            }
        }

        isFlipping = false;
    }
}