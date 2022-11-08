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
    [SerializeField] private Reader Chichay;
    
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
        
        controller.AddSequence(new WaitSequence(controller, 3.0f))
            // Visitor Request
            .AddSequence(new CustomSequence(controller, (s, o) =>
            {
                Chichay.ShowHideRequest(true);
                s.SetStatus(true);
            }))
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(254, 191f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Ito ang hinihingi ng bisita, konektado ito sa librong gusto niya. \n \n This is the visitor's request, this picture is the book that the visitor wants."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))

            // Patience Meter
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(-136, 270f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 1.5f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Ito ang 'Patience Meter'. Ibigay ang librong hinihingi bago maubos ang Patience Meter. \n \n This is the 'Patience Meter'. Give the right book before the patience meter runs out."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))
            
            // Book Mechanics
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(38, -603f), 5, rectTransform)) 
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang libro para makita ang deskripsyon. \n \n When a book is tapped, its description will appear."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang naka-highlight upang makita ang deskripsyon ng salita. \n \n Tap the highlighted word to show its description."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang labas libro upang isara ito. \n \n Tap outside of the book to close it."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ng dalawang beses ang libro para piliin ito. \n \n Double tap to select the book."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang 'Confirm' upang ibigay ang libro sa bisita. \n \n Tap 'Confirm' to give the book to the visitor."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "Pindutin ang labas ng libro para ikansela ang napili. \n \n Tap outside the book to cancel."))
            .AddSequence(new WaitSequence(controller, 2.0f))
            .AddSequence(us)
            .AddSequence(new ToolTipSequence(controller, _adapter, "", false))

            // Scoring
            .AddSequence(new MoveSequenceCanvas(controller, new Vector2(397, 706f), 5, rectTransform)) 
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