using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private UserInputSequence us;

    [SerializeField] private ToolTipAdapter _adapter;

    private void Awake()
    {
        var controller = GetComponent<SequenceController>();
        
        us = new UserInputSequence(controller);

        controller.AddSequence(new WaitSequence(controller, 5.0f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(136, -74f), 10, GetComponent<RectTransform>()))
            .AddSequence(new ToolTipSequence(controller, _adapter, "test"))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "no"))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(136, -74f), 10, GetComponent<RectTransform>()))
            .AddSequence(us)
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new CustomSequence(controller)
                .SetAction((s, o) =>
                {
                    o.GetComponent<SpriteRenderer>().enabled = false;
                }));

    }

    public void Test()
    {
        us.Toggle();
    }
    
}