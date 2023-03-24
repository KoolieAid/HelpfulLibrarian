using System.Collections.Generic;
using UnityEngine;
using Asyncoroutine;
using UnityEngine.Events;

public abstract class Mem_Book : MonoBehaviour
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

    [SerializeField] private SpriteRenderer cover;
    [SerializeField] private SpriteRenderer back;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        SetUp();
    }

    public abstract void SetUp();

    public void ChangeCovers(BookCover cover)
    {
        this.cover.sprite = cover.bookFront;
        back.sprite = cover.bookFront;
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

            if (
                (Mathf.Abs((float)targetState - transform.localRotation.eulerAngles.y) <= rotationPrecision) ||
                (Mathf.Abs((float)targetState - transform.localRotation.eulerAngles.y) >
                 358) // Some band aid fix that idk math to fix
            )
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