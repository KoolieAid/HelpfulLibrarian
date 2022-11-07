using System;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using Unity.VisualScripting;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private UserInputSequence us;
    
    [SerializeField] private ToolTipAdapter _adapter;
    
    /*
     * 1. Introduction
     * 2. Visitor Requests (Tooltip)
     * 3. Visitor Patience Meter
     * 4. Book Mechanics and Controls
     * 5. Scoring (Tooltip)
     */

    private void Awake()
    {
        var controller = GetComponent<SequenceController>();
        
        us = new UserInputSequence(controller);
        var rectTransform = GetComponent<RectTransform>();
        
        controller.AddSequence(new WaitSequence(controller, 5.0f))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(254, 191f), 5, rectTransform)) // Visitor Request
            .AddSequence(new WaitSequence(controller, 1.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Visitor Request Info"))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-136, 270f), 5, rectTransform)) // Patience Meter
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Patience Meter Info"))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(38, -603f), 5, rectTransform)) // Book Mechanics
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Book Mechanics Info"))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(397, 706f), 5, rectTransform)) // Scoring
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Scoring Info"))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            
            // end tutorial, go to game mode select scene
            
            .AddSequence(new CustomSequence(controller)
                .SetAction((s, o) =>
                {
                    o.SetActive(false);
                }));

    }

    public void Test()
    {
        us.Toggle();
    }
    
}